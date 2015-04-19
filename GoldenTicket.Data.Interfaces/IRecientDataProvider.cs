using System.Collections.Generic;
using GoldenTicket.Model;

namespace GoldenTicket.Data.Interfaces
{
    public interface IRecientDataProvider<TRawEntity> : IDataProvider<Recient, TRawEntity>
         where TRawEntity : class
    {
        IEnumerable<Recient> GetRecientItems();

        bool IsExisted(Recient item);

        void Save(Recient item);

        void SaveMany(IEnumerable<Recient> items);
    }
}
