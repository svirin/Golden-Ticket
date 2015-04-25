using System.Collections.Generic;
using GoldenTicket.Model;

namespace GoldenTicket.Data.Interfaces
{
    public interface IJoinDataProvider<TRawEntity> : IDataProvider<Join, TRawEntity>
         where TRawEntity : class
    {
        IEnumerable<Join> GetVisitsByUser(User user);
        bool IsExisted(Join item);

        void Save(Join item);
        void SaveMany(IEnumerable<Join> items);
    }
}
