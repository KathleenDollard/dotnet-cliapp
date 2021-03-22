
using CliSupport;
using Common;
using StarFruit2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetCli
{
    [Description("Run or manage templates")]
    public class NewCommand : DotnetCommand
    {
        [Description("The name for the output being created. If no name is specified, the name of the output directory is used.")]
        public string? Name { get; set; }

        [Description("Location to place the generated output.")]
        public string? Output { get; set; }
        [Description("Displays a summary of what would happen if the given command line were run if it would result in a template creation.")]
        public bool DryRun { get; set; }
        [Description("Forces content to be generated even if it would change existing files.")]
        public bool Force { get; set; }
        [Description("Filters the templates based on the template author.")]
        public string? Author { get; set; }
        [Description("The template to invoke.")]
        [Argument]
        public string? TemplateName { get; set; }

        public int Invoke()
        {
            return 0;
        }

        [Description("Lists templates containing the specified template name.If no name is specified, lists all templates.")]
        public  int ListCommand(
            [Description("Columns to display")]
            string[] columns,
            [Description("Filters templates based on available types.Predefined values are 'project' and 'item'.")]
            TemplateType type,
            [Description("Filters templates based on language and specifies the language of the template to create.")]
            [Aliases("--lang")]
            string language)
        {
            if (Help2)
            {
                return DisplayListHelp( UxLevel, RenderTo);
            }
            return DisplayList(UxLevel, RenderTo, columns, type, language );

            static int DisplayList(int uxLevel, RenderContext renderTo, string[] columns, TemplateType type, string language)
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
                    table.AddRow(template.TemplateName,
                                 template.ShortName,
                                 EscapeSquare(template.Language),
                                 template.Tags,
                                 template.Author,
                                 template.Type.ToString());
                }

                Cli.Render(table, renderTo, uxLevel, columns);

                return 0;
            }

            // NOTE: This is NOT the way this will be done when we integrate with System.CommandLine. This method will not exist. 
            static int DisplayListHelp(int uxLevel, RenderContext renderContext)
            {
                var rootCommandSource = CommandSource.Create<DotnetCommand>() as DotnetCommandCommandSource ;
                var commandSource = rootCommandSource?.NewCommandCommand.ListCommand;
                _ = commandSource ?? throw new InvalidOperationException();

                var command = commandSource.Command;
                var helpCommand = command.ToHelpCommand();
                Cli.Render(helpCommand, renderContext, uxLevel);

                return 0;
            }
        }

        // List2 method is to play with ideas around simplifying table interaction: 
        // * Identify table (via name and Roslyn? Hack with type?
        // * Include "columns" automatically? How to add this to render call? Dummy Render method?
        // * Allow columns to be marked for inclusion as option, allow alias name, maybe not additional
        // * Provide "order-by" by default (allow removal)
        [Description("Lists templates containing the specified template name.If no name is specified, lists all templates.")]
        [OutputsTable("table")]
        public  int List2Command(
            [Description("Columns to display")]
            string[] columns, // Disapears on directive approach and table definition
            [Description("Filters templates based on available types.Predefined values are 'project' and 'item'.")]
            TemplateType type,
            [Description("Filters templates based on language and specifies the language of the template to create.")]
            [Aliases("--lang")]
            string language)
        {
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

            Cli.Render(table, RenderTo, UxLevel, columns);

            return 0;
        }

        [Description("Searches for the templates in configured remote sources.")]
        public  int Search(
                    [Description("Columns to display")]
                    string[] columns,
                    [Description("Filters templates based on available types.Predefined values are 'project' and 'item'.")]
                    TemplateType type,
                    [Description("Filters templates based on language and specifies the language of the template to create.")]
                    [Aliases("--lang")]
                    string language,
                    [Description("Filters the templates based on NuGet package ID.Applicable only with --search option.")]
                    string packageId)
        {

            return 0;
        }


        [Description("Installs a source or a template pack.")]
        public  int InstallCommand()
        {

            return 0;
        }

        [Description("Uninstalls a source or a template pack.")]
        public  int UninstallCommand(
            [Description("Columns to display")]
            string[] columns)
        {

            return 0;
        }

        [Description("Check the currently installed template packs for updates.")]
        public  int CheckCommand()
        {
            return 0;
        }

        [Description("Check the currently installed template packs for update, and install the updates.")]
        public  int UpdateCommand()
        {
            return 0;
        }


        //"  --columns<COLUMNS_LIST> Comma separated list of columns to show for --list|-l or --search option.  The supported columns are:"
        //"                             - language - comma separated list of languages supported by the template"
        //"                             - tags - the list of template tags"
        //"                             - author - the template author"
        //"                             - type - the template type: project or item"
        //"                 The template name and short name are shown always."
        //"                 For --search option additionally NuGet package ID and total downloads count are shown always."
        //"                 The default list of columns shown without the option for --list | -l: template name, short name, language, tags; equivalent to --columns=language,tags."
        //"                 The default list of columns shown without the option for --search: template name, short name, author, language, package, total downloads; equivalent to --columns=author,language."
        //"  --columns-all Shows all available columns for --list | -l or --search option, equivalent to --columns=language,tags,author,type."

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
