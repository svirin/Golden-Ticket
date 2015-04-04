using GoldenTicket.Command.Interfaces;
using GoldenTicket.Model;

namespace GoldenTicket.Suggestion.Test
{
    public class CommandFactory : ICommandFactory<User>
    {
        public ICommand<User> CreateCommand()
        {
            return new Command();
        }
    }
}
