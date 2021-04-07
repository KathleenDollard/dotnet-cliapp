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
            AnsiConsole.Render(new Markup("\n"));
            if (uxLevel == 0)
            {
                DisplayLegacyStyle(help);
                return 0;
            }
            RenderDescription(uxLevel, help.RootHelpCommand);
            RenderHelpUsage(uxLevel, help.RootHelpCommand);
            RenderHelpPart(uxLevel, help.RootHelpCommand.Arguments);
            RenderHelpPart(uxLevel, help.RootHelpCommand.Options);
            RenderHelpPart(uxLevel, help.RootHelpCommand.SubCommands);

            RenderhelpSectionHead(uxLevel, null);

            return 0;
        }

        public static string? RenderPrompt(int uxLevel, Selector selector)
        {
            if (uxLevel == 0)
            {
                DisplayLegacyStyle(selector);
                return null;
            }
            var prompt = new SelectionPrompt<string>().Title($"Select the {selector.ItemTypeName} you'd like to use")
                                                      .PageSize(10);
            var selectorItems = selector.Items.Select(x => (x.Common ? "*" : "") + x.Value);
            if (selector.Sorted)
            {
                selectorItems = selectorItems.OrderBy(x => x);
            }
            prompt.AddChoices(selectorItems);
            var selectedItem = AnsiConsole.Prompt(prompt);
            if (selectedItem.StartsWith("*"))
            {
                selectedItem = selectedItem[1..];
            }
            return selectedItem;
        }

        public static int RenderPlainText(int uxLevel, string output)
        {
            if (uxLevel == 0)
            {
                DisplayLegacyStyle(output);
                return 0;
            }
            Console.WriteLine(output);
            return 0;
        }


        private static void DisplayLegacyStyle(object help)
        {
            throw new NotImplementedException();
        }

        private static void RenderDescription(int uxLevel, HelpCommand helpCommand)
        {
            RenderhelpSectionHead(uxLevel, "Description");
            AnsiConsole.Render(new Markup("  " + helpCommand?.Description + Environment.NewLine + Environment.NewLine));
        }

         private static void RenderhelpSectionHead(int uxLevel, string? text)
        {
            switch (uxLevel)
            {
                case 0: return;
                case 1:
                case 2:
                case 3:
                    AnsiConsole.Render(new Markup(text + ":"));
                    return;
                case 4:
                    if (string.IsNullOrWhiteSpace(text))
                    { AnsiConsole.Render(new Rule().Alignment(Justify.Left).RuleStyle("grey dim")); }
                    else
                    { AnsiConsole.Render(new Rule($"[olive dim]{text}[/]").Alignment(Justify.Left).RuleStyle("grey dim")); }
                    return;
                case 5:
                    if (string.IsNullOrWhiteSpace(text))
                    { AnsiConsole.Render(new Rule().Alignment(Justify.Left).RuleStyle("blue dim")); }
                    else
                    { AnsiConsole.Render(new Rule($"[white]{text}[/]").Alignment(Justify.Left).RuleStyle("blue dim")); }
                    return;
                default:
                    break;
            } 
        }

        private static void RenderHelpUsage(int uxLevel, HelpCommand helpCommand)
        {
            RenderhelpSectionHead(uxLevel, "Usage");
            renderHelpUsage(helpCommand);

            static void renderHelpUsage(HelpCommand helpCommand)
            {
                var ret = new List<string>();
                if (helpCommand.CanExecute)
                {
                    AnsiConsole.Render(new Markup("  " + BuildHelpUsageLine(helpCommand, null, true) + Environment.NewLine));
                }
                foreach (var subCommand in helpCommand.SubCommands)
                {
                    AnsiConsole.Render(new Markup("  " + BuildHelpUsageLine(helpCommand, subCommand, true) + Environment.NewLine));
                }
                AnsiConsole.Render(new Markup(Environment.NewLine));

            }

            static string BuildHelpUsageLine(HelpCommand helpCommand, HelpCommand? subCommand, bool includeParents)
            {
                var lowImpact = Color.Grey.ToString();
                var ret = includeParents && helpCommand.ParentCommandNames is not null
                            ? $"[{lowImpact}]{string.Join(".", helpCommand.ParentCommandNames)}[/]" 
                            : "";
                ret += $" {helpCommand.Name} ";
                ret += subCommand is null
                             ? usageFromParts(helpCommand, lowImpact)
                             : $" {BuildHelpUsageLine(subCommand!, null, false)}";
                return ret;

                static string usageFromParts(HelpCommand helpCommand, string lowImpact)
                {
                    return string.Join("", helpCommand.Arguments.Select(x => $"[{lowImpact}]<[/]{x.DisplayName}[{lowImpact}]>[/] "))
                              + (helpCommand.Options.Any() ? $"[{lowImpact}][[OPTIONS]][/]" : "");
                }
            }
        }

        private static int RenderHelpPart<T>(int uxLevel, HelpPart<T> helpPart)
            where T : HelpItem
        {
            if (!helpPart.Items.Any())
            {
                return 0;
            }
            RenderhelpSectionHead(uxLevel, helpPart.PartName);

            var anyAliases = helpPart.SelectMany(x => x.Aliases).Any(x => !string.IsNullOrWhiteSpace(x));
            Spectre.Console.Table helpPartTable = buildHelpPartTable(helpPart, anyAliases);
            SetHelpPartTableCharacteristics(uxLevel, helpPartTable);
            var panel = new Spectre.Console.Panel(helpPartTable);
            // Patrik: Apparent bug in Spectre console. This causes the columns wrapping to be off
            //panel.NoBorder().Padding(0, 0, 0, 0);
            //AnsiConsole.Render(panel);
            AnsiConsole.Render(helpPartTable );
            AnsiConsole.Render(new Markup(Environment.NewLine));
            return 0;

            static Spectre.Console.Table buildHelpPartTable<TItem>(HelpPart<TItem> helpPart, bool anyAliases) 
                where TItem : HelpItem
            {
                var helpPartTable = new Spectre.Console.Table().HideHeaders();
                var nameColumn = new Spectre.Console.TableColumn("");
                    //.PadLeft(5).PadRight(2);
                helpPartTable.AddColumn(nameColumn);
                if (anyAliases)
                {
                    helpPartTable.AddColumn("");
                }
                helpPartTable.AddColumn("");
                foreach (var item in helpPart.Items)
                {
                    helpPartTable.AddRow(helpRowForItem(item, anyAliases).ToArray());
                }

                return helpPartTable;
            }

            static IEnumerable<string> helpRowForItem(HelpItem option, bool anyAliases) 
                => anyAliases
                    ? new List<string>
                        {
                            "  " + option.DisplayName,
                            string.Join(", ", option.Aliases),
                            option.Description ?? "",
                        }
                    : new List<string>
                        {
                            "  " + option.DisplayName,
                            option.Description ?? "",
                        };

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
                    renderTable.Border(TableBorder.Minimal);
                    renderTable.BorderColor(Color.Olive);
                    renderTable.Expand();
                    return;
                case 3:
                    renderTable.Border(TableBorder.Horizontal);
                    renderTable.BorderColor(Color.Green);
                    renderTable.Expand();
                    return;
                case 4:
                    renderTable.Border(TableBorder.Rounded);
                    renderTable.BorderColor(Color.Grey);
                    renderTable.Collapse();
                    return;
                case 5:
                    renderTable.Border(TableBorder.Horizontal);
                    renderTable.BorderColor(Color.Navy);
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
                    helpPartTable.Border(TableBorder.Horizontal);
                    helpPartTable.BorderColor(Color.Olive);
                    helpPartTable.Expand();
                    return;
                case 4:
                    helpPartTable.Border(TableBorder.None);
                    helpPartTable.Collapse();
                    return;
                case 5:
                    helpPartTable.Border(TableBorder.Horizontal);
                    helpPartTable.BorderColor(Color.Olive);
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
                    helpUsageTable.Border(TableBorder.None);
                    helpUsageTable.BorderColor(Color.Blue);
                    helpUsageTable.Collapse();
                    return;
                case 5:
                    helpUsageTable.Border(TableBorder.None);
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
