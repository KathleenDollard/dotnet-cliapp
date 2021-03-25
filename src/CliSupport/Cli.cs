using Common;
using JsonRender;
using System;
using System.Linq;
using TerminalRender;

namespace CliSupport
{
    public static class Cli
    {
        private const int uxLevelEnvDefault = 4;
        private const RenderContext renderContextDefault = RenderContext.Terminal ;

        public static int Render(Table table, RenderContext renderTo = RenderContext.Unknown, int uxLevel = 0, params string[] columns)
        {
            renderTo = renderTo == RenderContext.Unknown ? GetRenderContext() : renderTo;
            uxLevel = uxLevel == 0 ? GetUxLevel() : uxLevel;
            AdjustColumns(table, columns);
            return renderTo switch
            {
                RenderContext.Terminal => Terminal.Render(uxLevel, table),
                RenderContext.Json => Json.Render(uxLevel, table),
                _ => throw new InvalidOperationException()
            };
        }

        public static string? GetFromPrompt(Selector selector, RenderContext renderTo, int uxLevel)
        {
            renderTo = renderTo == RenderContext.Unknown ? GetRenderContext() : renderTo;
            uxLevel = uxLevel == 0 ? GetUxLevel() : uxLevel;
            return renderTo switch
            {
                RenderContext.Terminal => Terminal.RenderPrompt(uxLevel, selector),
                _ => throw new InvalidOperationException()
            };
        }


        public static int Render(Help help, RenderContext context = RenderContext.Unknown, int uxLevel = 0)
        {
            context = context == RenderContext.Unknown ? GetRenderContext() : context;
            uxLevel = uxLevel == 0 ? GetUxLevel() : uxLevel;
            return context switch
            {
                RenderContext.Terminal => Terminal.Render(uxLevel, help),
                RenderContext.Json => Json.Render(uxLevel, help),
                _ => throw new InvalidOperationException()
            };
        }


        private static void AdjustColumns(Table table, string[] columns)
        {
            if (columns is null || !columns.Any())
            {
                foreach (var tableColumn in table.Columns.Where(col=>col.ColumnType != TableColumnType.Default 
                                                            && col.ColumnType!= TableColumnType.Mandatory))
                {
                    tableColumn.Hide = true;
                }
                return;
            }
            foreach (var tableColumn in table.Columns.Where(col => col.ColumnType != TableColumnType.Mandatory
                                            && !columns.Contains(col.Header)))
            {
                tableColumn.Hide = true;
            }
        }


        private static int GetUxLevel()
        {
            var uxLevelEnv = Environment.GetEnvironmentVariable("DOTNETCLI_UXLEVEL");
            if (int.TryParse(uxLevelEnv, out var result))
            {
                return result;
            }
            return uxLevelEnvDefault;
        }

        private static RenderContext GetRenderContext()
        {
            return RenderContext.Terminal;
        }
    }
}
