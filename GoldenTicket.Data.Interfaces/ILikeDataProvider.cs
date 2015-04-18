using System.Collections.Generic;
using GoldenTicket.Model;

namespace GoldenTicket.Data.Interfaces
{
    public interface ILikeDataProvider<TRawEntity> : IDataProvider<Like, TRawEntity>
         where TRawEntity : class
    {
        IEnumerable<Like> GetLikesByUser(User user);
        bool IsExisted(Like item);

        void Save(Like item);
        void SaveMany(IEnumerable<Like> items);
    }
}
