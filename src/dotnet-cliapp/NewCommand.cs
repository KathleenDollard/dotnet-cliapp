using CliSupport;
using Common;
using StarFruit2;
using System;
using System.Linq;

namespace DotnetCli
{
    /// <summary>
    /// Run or manage templates.
    /// </summary>
    [Description("")]
    public class NewCommand : DotnetCommand
    {
        /// <summary>
        /// The name for the output being created. If no name is specified, the name of the output directory is used.
        /// </summary>
        [Aliases("--n")]
        public string? Name { get; set; }

        /// <summary>
        /// Location to place the generated output.
        /// </summary>
        [Aliases("--o")]
        public string? Output { get; set; }

        /// <summary>
        /// Displays a summary of what would happen if the given command line were run if it would result in a template creation.
        /// </summary>
        public bool DryRun { get; set; }

        /// <summary>
        /// Forces content to be generated even if it would change existing files.
        /// </summary>
        public bool Force { get; set; }

        /// <summary>
        /// Filters the templates based on the template author.
        /// </summary>
        public string? Author { get; set; }

        /// <summary>
        /// The template to invoke.
        /// </summary>
        [Argument]
        public string? TemplateName { get; set; }

        public int Invoke()
        {
            // Remove when we incorporate into real help
            if (Help2)
            {
                var rootCommandSource = CommandSource.Create<DotnetCommand>() as DotnetCommandCommandSource;
                var commandSource = rootCommandSource?.NewCommandCommand;
                return DisplayListHelp(UxLevel, RenderTo, RenderToFileName, commandSource, true);
            }
            var commonTemplates = "console, classlib";
            var data = TemplateData.SampleData; // get real data here

            var selector = new Selector();
            selector.AddRange(data.Select(template => new SelectorItem(template.ShortName,
                                                                       template.TemplateName,
                                                                       commonTemplates.Contains(template.ShortName))));
            selector.Sorted = true;
            var shortName = Cli.GetFromPrompt(selector, RenderTo, UxLevel);
            Console.WriteLine(string.IsNullOrWhiteSpace(shortName)
                                ? "Enter a template name or a command"
                                : $"Run {shortName}");
            return 0;
        }

        /// <summary>
        /// Lists templates containing the specified template name.If no name is specified, lists all templates.
        /// </summary>
        /// <param name="columns">Columns to display</param>
        /// <param name="type">Filters templates based on available types.Predefined values are 'project' and 'item'.</param>
        /// <param name="language">Filters templates based on language and specifies the language of the template to create.</param>
        public int ListCommand(
            string[] columns,
            TemplateType type,
            [Aliases("--lang")]
            string language)
        {
            // Remove when we incorporate into real help
            if (Help2)
            {
                var rootCommandSource = CommandSource.Create<DotnetCommand>() as DotnetCommandCommandSource;
                var commandSource = rootCommandSource?.NewCommandCommand.ListCommand;
                return DisplayListHelp(UxLevel, RenderTo, RenderToFileName, commandSource);
            }
            return DisplayList(UxLevel, RenderTo, RenderToFileName, columns, type, language);

            static int DisplayList(int uxLevel, RenderContext renderTo, string? renderToFileName, string[] columns, TemplateType type, string language)
            {
                var data = TemplateData.SampleData; // get real data here
                if (type != TemplateType.Unknown)
                {
                    data = data.Where(x => x.Type == type).ToList();
                }
                if (!string.IsNullOrWhiteSpace(language))
                {
                    data = data.Where(x => string.Equals(x.Language, language, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                var table = new Table();
                table.AddColumn("Name", TableColumnType.Mandatory);
                table.AddColumn("Short Name", TableColumnType.Mandatory);
                table.AddColumn("Language");
                table.AddColumn("Tags");
                table.AddColumn("Author", TableColumnType.Optional);
                table.AddColumn("Type", TableColumnType.Optional);


                foreach (var template in data)
                {
                    table.AddRow(template.TemplateName, template.ShortName, EscapeSquare(template.Language),
                                 template.Tags, template.Author, template.Type.ToString());
                }

                Cli.Render(table, renderTo, uxLevel, outputFile: renderToFileName, columns: columns);

                return 0;
            }
        }

        // NOTE: This is NOT the way this will be done when we integrate with System.CommandLine Help. This method will not exist. 
        private static int DisplayListHelp(int uxLevel, RenderContext renderContext, string? renderToFile, CommandSourceBase? commandSource, bool canExecute = false)
        {
            _ = commandSource ?? throw new InvalidOperationException();

            var command = commandSource.Command;
            var helpCommand = command.ToHelpCommand(canExecute);
            Cli.Render(helpCommand, renderContext, uxLevel, outputFile: renderToFile);

            return 0;
        }

        /// <summary>
        /// Lists templates containing the specified template name.If no name is specified, lists all templates.
        /// </summary>
        /// <param name="columns">Columns to display</param>
        /// <param name="type">Filters templates based on available types.Predefined values are 'project' and 'item'.</param>
        /// <param name="language">Filters templates based on language and specifies the language of the template to create.</param>
        // List2 method is to play with ideas around simplifying table interaction: 
        // * Identify table (via name and Roslyn? Hack with type?
        // * Include "columns" automatically? How to add this to render call? Dummy Render method?
        // * Allow columns to be marked for inclusion as option, allow alias name, maybe not additional
        // * Provide "order-by" by default (allow removal)
        [OutputsTable("table")]
        public int List2Command(
            string[] columns, // Disapears on directive approach and table definition
            TemplateType type,
            [Aliases("--lang")]
            string language)
        {
            // Remove when we incorporate into real help
            if (Help2)
            {
                //return DisplayListHelp(UxLevel, RenderTo);
            }
            var table = new Table();
            table.AddColumn("Name", TableColumnType.Mandatory);
            table.AddColumn("Short Name", TableColumnType.Mandatory);
            table.AddColumn("Language");
            table.AddColumn("Tags");
            table.AddColumn("Author", TableColumnType.Optional);
            table.AddColumn("Type", TableColumnType.Optional);

            var data = TemplateData.SampleData; // get real data here

            foreach (var template in data)
            {
                table.AddRow(template.TemplateName,
                             template.ShortName,
                             EscapeSquare(template.Language),
                             template.Tags,
                             template.Author,
                             template.Type.ToString());
            }

            Cli.Render(table, RenderTo, UxLevel, outputFile: RenderToFileName, columns: columns);

            return 0;
        }

        /// <summary>
        /// Searches for the templates in configured remote sources.
        /// </summary>
        /// <param name="columns">Columns to display</param>
        /// <param name="type">Filters templates based on available types.Predefined values are 'project' and 'item'.</param>
        /// <param name="language">Filters templates based on language and specifies the language of the template to create.</param>
        /// <param name="packageId">Filters the templates based on NuGet package ID.Applicable only with --search option.</param>
        public int Search(
                    string[] columns,
                    TemplateType type,
                    [Aliases("--lang")]
                    string language,
                    string packageId)
        {

            // Remove when we incorporate into real help
            if (Help2)
            {
                throw new NotImplementedException();
            }
            return 0;
        }

        /// <summary>
        /// Installs a source or a template pack.
        /// </summary>
        public int InstallCommand()
        {
            // Remove when we incorporate into real help
            if (Help2)
            {
                throw new NotImplementedException();
            }
            return 0;
        }

        /// <summary>
        /// Uninstalls a source or a template pack.
        /// </summary>
        /// <param name="columns">Package Id to uninstall. Use 'dptmet new list --columns PackageId' to find package Id.</param>
        /// <returns></returns>
        public int UninstallCommand(
            string[] columns)
        {
            // Remove when we incorporate into real help
            if (Help2)
            {
                throw new NotImplementedException();
            }
            return 0;
        }

        /// <summary>
        /// Check the currently installed template packs for updates.
        /// </summary>
        /// <returns></returns>
        public int CheckCommand()
        {
            // Remove when we incorporate into real help
            if (Help2)
            {
                throw new NotImplementedException();
            }
            return 0;
        }

        /// <summary>
        /// Check the currently installed template packs for update, and install the updates.
        /// </summary>
        /// <returns></returns>
        public int UpdateCommand()
        {
            // Remove when we incorporate into real help
            if (Help2)
            {
                throw new NotImplementedException();
            }
            return 0;
        }

        /// <summary>
        /// Spectre.Console sees square brackets as markup. This will need to be stripped for outer output
        /// </summary>
        /// <param name="value"></param>
        /// <returns>[[ replaces [</returns>
        private static string EscapeSquare(string value)
            => value.Replace("[", "[[")
                    .Replace("]", "]]");

    }
}
