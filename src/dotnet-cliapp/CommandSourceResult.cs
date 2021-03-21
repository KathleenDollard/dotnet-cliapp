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
        public DotnetCommandCommandSourceResult(ParseResult parseResult, CommandSourceBase commandSource, int exitCode)
        : base(parseResult, commandSource, exitCode)
        {
        }

        public override DotnetCommand CreateInstance()
        {
            var newItem = new DotnetCommand();
            return newItem;
        }
    }
    public class NewCommandCommandSourceResult : DotnetCommandCommandSourceResult
    {
        public NewCommandCommandSourceResult(ParseResult parseResult, NewCommandCommandSource commandSource, int exitCode)
        : base(parseResult, commandSource, exitCode)
        {
        }

        public override NewCommand CreateInstance()
        {
            var newItem = new NewCommand();
            return newItem;
        }
    }

    public class ListCommandCommandSourceResult : NewCommandCommandSourceResult
    {
        public ListCommandCommandSourceResult(ParseResult parseResult, ListCommandSource commandSource, int exitCode)
        : base(parseResult, commandSource, exitCode)
        {
            UxLevelOption_Result = CommandSourceMemberResult.Create(commandSource.UxLevelOption, parseResult);
            RenderToOption_Result = CommandSourceMemberResult.Create(commandSource.RenderToOption, parseResult);
            ColumnsOption_Result = CommandSourceMemberResult.Create(commandSource.ColumnsOption, parseResult);
            TypeOption_Result = CommandSourceMemberResult.Create(commandSource.TypeOption, parseResult);
            LanguageOption_Result = CommandSourceMemberResult.Create(commandSource.LanguageOption, parseResult);
        }
        public CommandSourceMemberResult<int> UxLevelOption_Result { get; set; }
        public CommandSourceMemberResult<RenderContext> RenderToOption_Result { get; set; }
        public CommandSourceMemberResult<string[]> ColumnsOption_Result { get; set; }
        public CommandSourceMemberResult<TemplateType> TypeOption_Result { get; set; }
        public CommandSourceMemberResult<string> LanguageOption_Result { get; set; }

        public override NewCommand CreateInstance()
        {
            var newItem = new NewCommand();
            return newItem;
        }

        public override int Run()
        {
            var newInstance = CreateInstance();
            return newInstance.ListCommand(UxLevelOption_Result.Value,
                                    RenderToOption_Result.Value,
                                    ColumnsOption_Result.Value,
                                    TypeOption_Result.Value,
                                    LanguageOption_Result.Value);
        }
    }

}
