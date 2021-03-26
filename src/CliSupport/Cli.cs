using Common;
using JsonRender;
using System;
using System.IO;
using System.Linq;
using TerminalRender;

namespace CliSupport
{
    public static class Cli
    {
        private const int uxLevelEnvDefault = 4;

        public static int Render(Table table,
                                 RenderContext renderTo = RenderContext.Unknown,
                                 int uxLevel = 0,
                                 string? outputFile = null,
                                 params string[] columns)
        {
            renderTo = renderTo == RenderContext.Unknown
                        ? GetRenderContext(outputFile)
                        : renderTo;
            uxLevel = uxLevel == 0 ? GetUxLevel() : uxLevel;
            AdjustColumns(table, columns);
            switch (renderTo)
            {
                case RenderContext.Terminal:
                    return Terminal.Render(uxLevel, table);
                case RenderContext.Json:
                    var output = Json.Render(uxLevel, table);
                    return string.IsNullOrWhiteSpace(outputFile)
                        ? Terminal.RenderPlainText(uxLevel, output)
                        : OutputToFile(output, outputFile);
                default:
                    throw new InvalidOperationException();
            }
        }

        public static string? GetFromPrompt(Selector selector, RenderContext renderTo, int uxLevel)
        {
            uxLevel = uxLevel == 0 
                        ? GetUxLevel() 
                        : uxLevel;
            return renderTo switch
            {
                RenderContext.Terminal or RenderContext.Unknown => Terminal.RenderPrompt(uxLevel, selector),
                _ => throw new InvalidOperationException()
            };
        }


        public static int Render(Help help,
                                 RenderContext renderTo = RenderContext.Unknown,
                                 int uxLevel = 0,
                                 string? outputFile = null)
        {
            renderTo = renderTo == RenderContext.Unknown 
                                        ? GetRenderContext(outputFile) 
                                        : renderTo;
            uxLevel = uxLevel == 0 ? GetUxLevel() : uxLevel;
            switch (renderTo)
            {
                case RenderContext.Terminal:
                    return Terminal.Render(uxLevel, help);
                case RenderContext.Json:
                    var output = Json.Render(uxLevel, help);
                    return string.IsNullOrWhiteSpace(outputFile)
                        ? Terminal.RenderPlainText(uxLevel, output)
                        : OutputToFile(output, outputFile);
                default:
                    throw new InvalidOperationException();
            }
        }

        private static int OutputToFile(string output, string outputFile)
        {
            throw new NotImplementedException();
        }

        private static void AdjustColumns(Table table, string[] columns)
        {
            if (columns is null || !columns.Any())
            {
                foreach (var tableColumn in table.Columns.Where(col => col.ColumnType != TableColumnType.Default
                                                            && col.ColumnType != TableColumnType.Mandatory))
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

        private static RenderContext GetRenderContext(string? renderToFile)
        {
            return string.IsNullOrWhiteSpace(renderToFile)
                    ? RenderContext.Terminal
                    : RenderContextFromFileName(renderToFile);

            static RenderContext RenderContextFromFileName(string renderToFile)
            {
                var ext = Path.GetExtension(renderToFile);
                return ext.Equals("JSON", StringComparison.OrdinalIgnoreCase)
                          ? RenderContext.Json
                          : RenderContext.Terminal; // just output what is displayed
            }
        }
    }
}
