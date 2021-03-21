using StarFruit2;
using System.CommandLine;
using StarFruit2.Common;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System;
using Common;

namespace DotnetCli
{
    public class DotnetCommandCommandSource : RootCommandSource<DotnetCommandCommandSource>
    {
        public DotnetCommandCommandSource()
        : this(null, null)
        {        }

        public DotnetCommandCommandSource(RootCommandSource root, CommandSourceBase parent)
        : base(new Command("dotnet", ""), parent)
        {
            NewCommandCommand = new NewCommandCommandSource(this, this);
            Command.AddCommand(NewCommandCommand.Command);
            Help2Option = GetHelp2Option();
            Command.Add(Help2Option);
            Command.Handler = CommandHandler.Create((InvocationContext context) =>
            {
                CurrentCommandSource = this;
                CurrentParseResult = context.ParseResult;
                return 0;
            });
        }

        public NewCommandCommandSource NewCommandCommand { get; set; }
        public Option<bool> Help2Option { get; private set; }

        public static Option<bool> GetHelp2Option()
        {
            Option<bool> option = new("--help2");
            option.AddAlias("-h2");
            option.Description = "Display help (new view).";
            return option;
        }
    }

    public class NewCommandCommandSource : DotnetCommandCommandSource
    {
        public NewCommandCommandSource(RootCommandSource root, DotnetCommandCommandSource parent)
        : base(new Command("new", ""), parent)
        {
            ListCommand = new ListCommandSource(root, this);
            Command.AddCommand(ListCommand.Command);
            Command.Handler = CommandHandler.Create(() =>
            {
                root.CurrentCommandSource = this;
                return 0;
            });
        }

        public ListCommandSource ListCommand { get; set; }

    }

    public class ListCommandSource : NewCommandCommandSource
    {
        public ListCommandSource(RootCommandSource root, NewCommandCommandSource parent)
        : base(new Command("list", ""), parent)
        {
            UxLevelOption = GetUxLevelOption();
            Command.Add(UxLevelOption);
            RenderToOption = GetRenderToOption();
            Command.Add(RenderToOption);
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

        public Option<int> UxLevelOption { get; set; }
        public Option<RenderContext> RenderToOption { get; set; }
        public Option<string[]> ColumnsOption { get; set; }
        public Option<TemplateType> TypeOption { get; set; }
        public Option<string> LanguageOption { get; set; }

        public static Option<int> GetUxLevelOption()
        {
            Option<int> option = new("--ux-level");
            option.Description = "Also set via Environment variable. 0 for none -> 5 for richest.";
            return option;
        }

        public static Option<RenderContext> GetRenderToOption()
        {
            Option<RenderContext> option = new("--render-to");
            option.Description = "Terminal or Json. Terminal is default.";
            return option;
        }

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
