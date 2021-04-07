using Common;
using System.CommandLine;
using System.Linq;

namespace CliSupport
{
    public static class Extensions
    {
        public static Help ToHelpCommand(this Command command, bool canExecute = false) 
            => new(command, CreateHelpCommand(command, canExecute: canExecute));

        private static HelpCommand CreateHelpCommand(Command command, HelpCommand? parent = null, bool canExecute = false)
        {
            var helpCommand = new HelpCommand(command.Name, command.Description)
            {
                ParentCommandNames = command.Parents.Select(x => x.Name).ToList(),
                CanExecute = canExecute
            };

            foreach (var argument in command.Arguments)
            {
                helpCommand.Arguments.Add(argument.Name, argument.Description);
            }
            foreach (var option in command.Options)
            {
                var newHelpOption = helpCommand.Options.Add(option.Aliases.First(), option.Description);
                foreach (var alias in option.Aliases.Skip(1))
                {
                    newHelpOption.Aliases.Add(alias);
                }
            }
            foreach (var subCommand in command.Children.OfType<Command>())
            {
                var helpSubCommand = helpCommand.SubCommands.Add(subCommand.Aliases.First().Trim(), subCommand.Description);
                var newHelpCommand = CreateHelpCommand(subCommand, helpCommand);
                foreach (var alias in subCommand.Aliases.Skip(1))
                {
                    newHelpCommand.Aliases.Add(alias.Trim());
                }
            }
            return helpCommand;
        }
    }
}
