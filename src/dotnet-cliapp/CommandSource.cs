using StarFruit2;
using System.CommandLine;
using StarFruit2.Common;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System;
using Common;

namespace DotnetCli
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is built as a container, while both the user's model and the result are built as 
    /// derived classes. That is because the other two cases work with a single result, and 
    /// ask questions of it and it's parent commands regarding data. These classes contain the 
    /// full model, and thus must be a container. 
    /// </remarks>
    public class DotnetCommandCommandSource : RootCommandSource<DotnetCommandCommandSource>
    {
        public DotnetCommandCommandSource()
        : this(null, null)
        { }

        public DotnetCommandCommandSource(RootCommandSource root, CommandSourceBase parent)
        : base(new Command("dotnet", "Access commands in support of .NET"), parent)
        {
            if (root is not null && root != this)
            {
                throw new InvalidOperationException();
            }
            NewCommandCommand = new NewCommandCommandSource(this, this);
            Command.AddCommand(NewCommandCommand.Command);
            Help2Option = GetHelp2Option();
            Command.Add(Help2Option);
            UxLevelOption = GetUxLevelOption();
            Command.Add(UxLevelOption);
            RenderToOption = GetRenderToOption();
            Command.Add(RenderToOption);
            Command.Handler = CommandHandler.Create((InvocationContext context) =>
            {
                CurrentCommandSource = this;
                CurrentParseResult = context.ParseResult;
                var result = new DotnetCommandCommandSourceResult(context.ParseResult, this, 0);
                result.Run();
                return 0;
            });
        }

        public NewCommandCommandSource NewCommandCommand { get; set; }
        public Option<bool> Help2Option { get; private set; }
        public Option<int> UxLevelOption { get; set; }
        public Option<RenderContext> RenderToOption { get; set; }

        public static Option<bool> GetHelp2Option()
        {
            Option<bool> option = new("--help2");
            option.AddAlias("-h2");
            option.Description = "Display help (new view).";
            return option;
        }

        public static Option<int> GetUxLevelOption()
        {
            Option<int> option = new("--ux-level");
            option.Description = "Gets the UI appearance. 0 for none -> 5 for richest.";
            return option;
        }

        public static Option<RenderContext> GetRenderToOption()
        {
            Option<RenderContext> option = new("--render-to");
            option.Description = "Terminal or Json. Terminal is default.";
            return option;
        }
    }

    public class NewCommandCommandSource : CommandSourceBase
    {
        public NewCommandCommandSource(RootCommandSource root, DotnetCommandCommandSource parent)
        : base(new Command("new", "Run or manage templates"), parent)
        {
            ListCommand = new ListCommandCommandSource(root, this);
            Command.AddCommand(ListCommand.Command);

            NameOption = GetNameOption();
            Command.Add(NameOption);
            OutputOption = GetOutputOption();
            Command.Add(OutputOption);

            Command.Handler = CommandHandler.Create((InvocationContext context) =>
            {
                root.CurrentCommandSource = this;
                var result = new NewCommandCommandSourceResult(context.ParseResult, this, 0);
                result.Run();
                return 0;
            });
        }

        public Option<string> NameOption { get; set; }
        public Option<string> OutputOption { get; set; }

        private static Option<string> GetNameOption()
        {
            Option<string> option = new("--name");
            option.AddAlias("-n");
            option.Description = "Name for the output being created. If not specified, the name of the output directory is used.";
            return option;
        }

        private static Option<string> GetOutputOption()
        {
            Option<string> option = new("--output");
            option.AddAlias("-o");
            option.Description = "Location to place the generated output.";
            return option;
        }

        public ListCommandCommandSource ListCommand { get; set; }

    }

    public class ListCommandCommandSource : CommandSourceBase
    {
        public ListCommandCommandSource(RootCommandSource root, NewCommandCommandSource parent)
        : base(new Command("list", "Lists templates containing the specified template name.If no name is specified, lists all templates."), parent)
        {
            ColumnsOption = GetColumnsOption();
            Command.Add(ColumnsOption);
            TypeOption = GetTemplateTypeOption();
            Command.Add(TypeOption);
            LanguageOption = GetLanguageOption();
            Command.Add(LanguageOption);

            Command.Handler = CommandHandler.Create((InvocationContext context) =>
            {
                root.CurrentCommandSource = this;
                var result = new ListCommandCommandSourceResult(context.ParseResult, this, 0);
                result.Run();
                return 0;
            });
        }

        public Option<string[]> ColumnsOption { get; set; }
        public Option<TemplateType> TypeOption { get; set; }
        public Option<string> LanguageOption { get; set; }

        private static Option<string[]> GetColumnsOption()
        {
            Option<string[]> option = new("--columns");
            option.Description = "Columns to display.";
            return option;
        }

        private static Option<TemplateType> GetTemplateTypeOption()
        {
            Option<TemplateType> option = new("--type");
            option.Description = "Includes only templates of this type.";
            return option;
        }

        private static Option<string> GetLanguageOption()
        {
            Option<string> option = new("--language");
            option.Description = "Includes only templates for this language.";
            return option;
        }
    }
}
