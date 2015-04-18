using System.Collections.Generic;
using GoldenTicket.Model;

namespace GoldenTicket.Data.Interfaces
{
    public interface ISuggestDataProvider<TRawEntity> : IDataProvider<Suggest, TRawEntity>
         where TRawEntity : class
    {
        IEnumerable<Suggest> GetSuggestByUser(User user);
        bool IsExisted(Suggest item);

        void Save(Suggest item);
        void SaveMany(IEnumerable<Suggest> items);
    }
}
