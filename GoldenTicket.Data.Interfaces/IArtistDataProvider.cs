using GoldenTicket.Model;
using System.Collections.Generic;

namespace GoldenTicket.Data.Interfaces
{
    public interface IArtistDataProvider<TRawEntity> : IDataProvider<Artist, TRawEntity>
         where TRawEntity : class
    {
        bool IsExisted(string name);

        IEnumerable<Artist> GetAcctualArtists();
    }
}
