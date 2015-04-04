using GoldenTicket.Command.Interfaces;
using GoldenTicket.Model;

namespace GoldenTicket.QueryAnalyzer.ArtistCopier
{
    public class CommandFactory : ICommandFactory<UserRequest>
    {
        public ICommand<UserRequest> CreateCommand()
        {
            return new Command();
        }
    }
}
