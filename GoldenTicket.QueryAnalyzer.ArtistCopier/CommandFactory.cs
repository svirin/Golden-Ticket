using GoldenTicket.Command.Interfaces;
using GoldenTicket.Model;

namespace GoldenTicket.QueryAnalyzer.ArtistCopier
{
    public class CommandFactory : ICommandFactory<Request>
    {
        public ICommand<Request> CreateCommand()
        {
            return new Command();
        }
    }
}
