using Common;
using System;
using Spectre.Console;
using System.Linq;
using System.Collections.Generic;

namespace TerminalRender
{
    public static class Terminal
    {
        public static int Render(int uxLevel, Common.Table table)
        {
            if (uxLevel == 0)
            {
                DisplayLegacyStyle(table);
                return 0;
            }
            var renderTable = new Spectre.Console.Table();
            BuildTable(renderTable, table);
            SetTableCharacteristics(renderTable, uxLevel);

            AnsiConsole.Render(renderTable);
            return 0;
        }

        public static int Render(int uxLevel, Help help)
        {
            if (uxLevel == 0)
            {
                DisplayLegacyStyle(help);
                return 0;
            }
            RenderHelpUsage(uxLevel, help.RootHelpCommand);
            RenderHelpPart(uxLevel, help.RootHelpCommand.Arguments);
            RenderHelpPart(uxLevel, help.RootHelpCommand.Options);
            RenderHelpPart(uxLevel, help.RootHelpCommand.SubCommands);

            return 0;
        }

        private static void RenderSectionHead(int uxLevel, string text)
        {
            switch (uxLevel)
            {
                case 0: return;
                case 1:
                    AnsiConsole.Render(new Markup(text));
                    return;
                case 2:
                    AnsiConsole.Render(new Markup(text));
                    return;
                case 3:
                    AnsiConsole.Render(new Markup(text));
                    return;
                case 4:
                    AnsiConsole.Render(new Rule(text).Alignment(Justify.Left));
                    return;
                case 5:
                    AnsiConsole.Render(new Rule("[white]{text}[/]").Alignment(Justify.Left).RuleStyle("red dim"));
                    return;

                default:
                    break;
            }
        }

        private static void RenderHelpUsage(int uxLevel, HelpCommand helpCommand)
        {
            var renderTable = BuildHelpUsage(helpCommand);

            RenderSectionHead(uxLevel, "Usage");
            SetHelpUsageTableCharacteristics(uxLevel, renderTable);

            AnsiConsole.Render(new Rule("Usage").Alignment(Justify.Left));

            static Spectre.Console.Table BuildHelpUsage(HelpCommand helpCommand)
            {
                var table = new Spectre.Console.Table();
                if (helpCommand.CanExecute)
                {
                    table.AddRow(BuildHelpUsageLine(helpCommand, null, true));
                }
                foreach (var subCommand in helpCommand.SubCommands)
                {
                    table.AddRow(BuildHelpUsageLine(helpCommand, subCommand, true));
                }
                return table;
            }

            static string BuildHelpUsageLine(HelpCommand helpCommand, HelpCommand? subCommand, bool includeParents)
            {
                var lowImpact = Color.Grey.ToString();
                var ret = includeParents ? $"[{lowImpact}]{string.Join(".", helpCommand.ParentCommandNames)}[/]" : ""
                           + $".{helpCommand.Name}"
                           + subCommand is null
                               ? usageFromParts(helpCommand, lowImpact)
                               : $" {BuildHelpUsageLine(subCommand!, null, false)}";
                return ret;

                static string usageFromParts(HelpCommand helpCommand, string lowImpact)
                {
                    return string.Join("", helpCommand.Arguments.Select(x => $"[{lowImpact}]<[/]{x.DisplayName}[{lowImpact}]>[/] "))
                              + string.Join("", helpCommand.Options.Select(x => $"[{lowImpact}]--[/]{x.DisplayName} "))
                              + string.Join("", helpCommand.SubCommands.Select(x => $"{x.DisplayName}..."));
                }
            }
        }

        private static int RenderHelpPart<T>(int uxLevel, HelpPart<T> helpPart)
            where T : HelpItem
        {
            var anyAliases = helpPart.SelectMany(x => x.Aliases).Any(x => string.IsNullOrWhiteSpace(x));
            var helpPartTable = new Spectre.Console.Table();
            helpPartTable.AddColumn("");
            helpPartTable.AddColumn("");
            if (anyAliases)
            {
                helpPartTable.AddColumn("");
            }
            SetHelpPartTableCharacteristics(uxLevel, helpPartTable);
            foreach (var item in helpPart.Items)
            {

                helpPartTable.AddRow((item switch
                {
                    HelpArgument arg => helpRowForArgument(arg),
                    HelpOption opt => helpRowForOption(opt, anyAliases),
                    HelpCommand cmd => helpRowForCommand(cmd, anyAliases),
                    _ => throw new InvalidOperationException()
                }).ToArray());
            }
            AnsiConsole.Render(helpPartTable);
            return 0;

            static string[] helpRowForArgument(HelpArgument argument)
                => new string[]
                        {
                            argument.DisplayName,
                            argument.Description ?? ""
                        };

            static IEnumerable<string> helpRowForOption(HelpOption option, bool anyAliases)
            {
                var ret = new List<string>
                        {
                            option.DisplayName,
                            option.Description ?? "",
                        };
                if (anyAliases)
                {
                    ret.Add(string.Join(", ", option.Aliases));
                }
                return ret;
            }

            static IEnumerable<string> helpRowForCommand(HelpCommand command, bool anyAliases)
            {
                var ret = new List<string>
                        {
                            command.DisplayName,
                            command.Description ?? "",
                        };
                if (anyAliases)
                {
                    ret.Add(string.Join(", ", command.Aliases));
                }
                return ret;
            }
        }


        private static void BuildTable(Spectre.Console.Table renderTable, Common.Table table)
        {
            foreach (var column in table.Columns)
            {
                if (!column.Hide)
                {
                    var renderColumn = new Spectre.Console.TableColumn(column.Header)
                    {
                        NoWrap = true
                    };
                    renderTable.AddColumn(renderColumn);
                }
            }

            foreach (var row in table.Rows)
            {
                var values = new List<Markup>();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (!table.Columns[i].Hide)
                    {
                        values.Add(new Markup(row[i], null).Overflow(Overflow.Ellipsis));
                    }
                }

                renderTable.AddRow(new Spectre.Console.TableRow(values));
            }
        }

        private static void SetTableCharacteristics(Spectre.Console.Table renderTable, int uxLevel)
        {
            switch (uxLevel)
            {
                case 0: return;
                case 1:
                    renderTable.Border(TableBorder.None);
                    renderTable.Expand();
                    return;
                case 2:
                    renderTable.Border(TableBorder.None);
                    renderTable.Expand();
                    return;
                case 3:
                    renderTable.Border(TableBorder.None);
                    renderTable.Expand();
                    return;
                case 4:
                    renderTable.Border(TableBorder.MinimalHeavyHead);
                    renderTable.BorderColor(Color.Blue);
                    renderTable.Collapse();
                    return;
                case 5:
                    renderTable.Border(TableBorder.MinimalHeavyHead);
                    renderTable.BorderColor(Color.Yellow);
                    renderTable.Collapse();
                    return;

                default:
                    break;
            }
        }

        private static void SetHelpPartTableCharacteristics(int uxLevel, Spectre.Console.Table helpPartTable)
        {
            switch (uxLevel)
            {
                case 0: return;
                case 1:
                    helpPartTable.Border(TableBorder.None);
                    helpPartTable.Expand();
                    return;
                case 2:
                    helpPartTable.Border(TableBorder.None);
                    helpPartTable.Expand();
                    return;
                case 3:
                    helpPartTable.Border(TableBorder.None);
                    helpPartTable.Expand();
                    return;
                case 4:
                    helpPartTable.Border(TableBorder.MinimalHeavyHead);
                    helpPartTable.BorderColor(Color.Blue);
                    helpPartTable.Collapse();
                    return;
                case 5:
                    helpPartTable.Border(TableBorder.MinimalHeavyHead);
                    helpPartTable.BorderColor(Color.Yellow);
                    helpPartTable.Collapse();
                    return;

                default:
                    break;
            }
        }

        private static void SetHelpUsageTableCharacteristics(int uxLevel, Spectre.Console.Table helpUsageTable)
        {
            switch (uxLevel)
            {
                case 0: return;
                case 1:
                    helpUsageTable.Border(TableBorder.None);
                    helpUsageTable.Expand();
                    return;
                case 2:
                    helpUsageTable.Border(TableBorder.None);
                    helpUsageTable.Expand();
                    return;
                case 3:
                    helpUsageTable.Border(TableBorder.None);
                    helpUsageTable.Expand();
                    return;
                case 4:
                    helpUsageTable.Border(TableBorder.MinimalHeavyHead);
                    helpUsageTable.BorderColor(Color.Blue);
                    helpUsageTable.Collapse();
                    return;
                case 5:
                    helpUsageTable.Border(TableBorder.MinimalHeavyHead);
                    helpUsageTable.BorderColor(Color.Yellow);
                    helpUsageTable.Collapse();
                    return;

                default:
                    break;
            }
        }

        private static void DisplayLegacyStyle(Common.Table table)
        {
            Console.WriteLine("Still need to get sizing from Template Engine");
        }
        private static void DisplayLegacyStyle(Help help)
        {
            Console.WriteLine("Still need to get sizing from Template Engine");
        }

    }
}
