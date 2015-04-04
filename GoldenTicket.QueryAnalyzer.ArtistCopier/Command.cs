using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GoldenTicket.Command.Interfaces;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using Parse;

namespace GoldenTicket.QueryAnalyzer.ArtistCopier
{
    public class Command : ICommand<UserRequest>
    {
        private IArtistDataProvider<ParseObject> _artistDataProvider;

        public void ExecuteCommand(UserRequest item)
        {
            _artistDataProvider = DI.Factory.GetInstance<IArtistDataProvider<ParseObject>>();

            var isArtistExisted = _artistDataProvider.IsExisted(item.Artist);

            if (!isArtistExisted)
            {
                // TODO : Need get artist's details from any enrich service 
                var artist = new Artist
                {
                    Name = item.Artist,
                };

                _artistDataProvider.Save(artist);
            }
        }
    }
}
