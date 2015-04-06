using GoldenTicket.Command.Interfaces;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using Parse;

namespace GoldenTicket.QueryAnalyzer.ArtistCopier
{
    public class Command : ICommand<Request>
    {
        public void ExecuteCommand(Request item)
        {
            var artistDataProvider = DI.Factory.GetInstance<IArtistDataProvider<ParseObject>>();
            var requestDataProvider = DI.Factory.GetInstance<IRequestDataProvider<ParseObject>>();

            var isArtistExisted = artistDataProvider.IsExisted(item.Artist);

            if (!isArtistExisted)
            {
                //TODO : Need get artist's details from any enrich service 
                var artist = new Artist
                {
                    Name = item.Artist,
                };

                artistDataProvider.Save(artist);

                requestDataProvider.ActivateRequest(item);
            }
        }
    }
}
