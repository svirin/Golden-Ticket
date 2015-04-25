using GoldenTicket.Command.Interfaces;
using GoldenTicket.Model;

namespace GoldenTicket.Crawler.Test
{
    public class CommandFactory : ICommandFactory<Artist>
    {
        public ICommand<Artist> CreateCommand()
        {
            return new Command();
        }
    }
}
