using StarFruit2;
using System.CommandLine;
using StarFruit2.Common;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System;
using Common;

namespace DotnetCli
{
    public class DotnetCommandCommandSourceResult : CommandSourceResult<DotnetCommand>
    {
        public DotnetCommandCommandSourceResult(ParseResult parseResult, CommandSourceBase? commandSourceBase, int exitCode)
        : base(parseResult, commandSourceBase, exitCode)
        {
            var commandSource = commandSourceBase as DotnetCommandCommandSource;
            if (commandSource is null)
            {
                throw new InvalidOperationException();
            }
            // Setup data (noting following means no data was expected)
            UxLevelOption_Result = CommandSourceMemberResult.Create(commandSource.UxLevelOption, parseResult);
            RenderToOption_Result = CommandSourceMemberResult.Create(commandSource.RenderToOption, parseResult);
            Help2Option_Result = CommandSourceMemberResult.Create(commandSource.Help2Option, parseResult);
        }

        // Result data 
        public CommandSourceMemberResult<int> UxLevelOption_Result { get; set; }
        public CommandSourceMemberResult<RenderContext> RenderToOption_Result { get; set; }
        public CommandSourceMemberResult<bool> Help2Option_Result { get; set; }

        protected override void FillInstance(DotnetCommand newInstance)
        {
            var newDotnetCommand = newInstance as DotnetCommand;
            if (newDotnetCommand is null)
            {
                throw new InvalidOperationException();
            }
            newDotnetCommand.UxLevel = UxLevelOption_Result.Value;
            newDotnetCommand.RenderTo = RenderToOption_Result.Value;
            newDotnetCommand.Help2 = Help2Option_Result.Value;
            base.FillInstance(newInstance);
        }

        public override DotnetCommand CreateInstance()
        {
            var newItem = new DotnetCommand();
            return newItem;
        }
    }
    public class NewCommandCommandSourceResult : DotnetCommandCommandSourceResult
    {
        public NewCommandCommandSourceResult(ParseResult parseResult, CommandSourceBase? commandSourceBase, int exitCode)
        : base(parseResult, commandSourceBase?.ParentCommandSource, exitCode)
        {
            var commandSource = commandSourceBase as NewCommandCommandSource;
            if (commandSource is null)
            {
                throw new InvalidOperationException();
            }
            // Setup data (noting following means no data was expected)

            // Setup data for properties
            Name_Result = CommandSourceMemberResult.Create(commandSource.NameOption, parseResult);
            Output_Result = CommandSourceMemberResult.Create(commandSource.OutputOption, parseResult);
        }

        // Result data 
        public CommandSourceMemberResult<string> Name_Result { get; set; }
        public CommandSourceMemberResult<string> Output_Result { get; set; }

        protected override void FillInstance(DotnetCommand newInstance)
        {
            var newNewCommand = newInstance as NewCommand;
            if (newNewCommand is null)
            {
                throw new InvalidOperationException();
            }
            newNewCommand.Name = Name_Result.Value;
            newNewCommand.Output = Output_Result.Value;
            base.FillInstance(newInstance);
        }

        public override NewCommand CreateInstance()
        {
            var newInstance = new NewCommand();
            FillInstance(newInstance);
            return newInstance;
        }

        public override int Run()
        {
            var newInstance = CreateInstance();
            return newInstance.Invoke();
        }
    }

    public class ListCommandCommandSourceResult : NewCommandCommandSourceResult
    {
        public ListCommandCommandSourceResult(ParseResult parseResult, CommandSourceBase? commandSourceBase, int exitCode)
        : base(parseResult, commandSourceBase.ParentCommandSource, exitCode)
        {
            var commandSource = commandSourceBase as ListCommandCommandSource;
            if (commandSource is null)
            {
                throw new InvalidOperationException();
            }
            // Setup data for parameters (noting following means no data was expected)
            ColumnsOption_Result = CommandSourceMemberResult.Create(commandSource.ColumnsOption, parseResult);
            TypeOption_Result = CommandSourceMemberResult.Create(commandSource.TypeOption, parseResult);
            LanguageOption_Result = CommandSourceMemberResult.Create(commandSource.LanguageOption, parseResult);

            // Setup data for properties
        }

        // Result data 
        public CommandSourceMemberResult<string[]> ColumnsOption_Result { get; set; }
        public CommandSourceMemberResult<TemplateType> TypeOption_Result { get; set; }
        public CommandSourceMemberResult<string> LanguageOption_Result { get; set; }

        public override int Run()
        {
            var newInstance = CreateInstance();
            return newInstance.ListCommand(
                                    ColumnsOption_Result.Value,
                                    TypeOption_Result.Value,
                                    LanguageOption_Result.Value);
        }
    }

}
