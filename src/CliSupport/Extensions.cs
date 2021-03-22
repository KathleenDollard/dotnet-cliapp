using Common;
using System.CommandLine;
using System.Linq;

namespace CliSupport
{
    public static class Extensions
    {
        public static Help ToHelpCommand(this Command command)
        {
            var help = new Help(command);
            help.RootHelpCommand = new HelpCommand(command.Name, command.Description);

            help.RootHelpCommand = CreateHelpCommand(command);

            return help;
        }

        private static HelpCommand CreateHelpCommand(Command command, HelpCommand? parent=null)
        {
            var helpCommand =  new HelpCommand(command.Name, command.Description);
            // This is not the correct way to set CanExecute and fails on dotnet new
            helpCommand.CanExecute = !command.Children.OfType<Command>().Any();
            helpCommand.ParentCommandNames  = command.Parents.Select(x=>x.Name).ToList();

            foreach (var argument in command.Arguments)
            {
                helpCommand.Arguments.Add(argument.Name, argument.Description);
            }
            foreach (var option in command.Options)
            {
                helpCommand.Options.Add(option.Name, option.Description);
            }
            foreach (var subCommand in command.Children.OfType<Command>())
            {
                var helpSubCommand = helpCommand.SubCommands.Add(subCommand.Name, subCommand.Description);
                CreateHelpCommand(subCommand, helpSubCommand );
            }
            return helpCommand;
        }
    }
}
