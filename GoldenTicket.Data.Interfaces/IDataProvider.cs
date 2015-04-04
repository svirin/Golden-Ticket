using System.Collections.Generic;
using GoldenTicket.Model;

namespace GoldenTicket.Data.Interfaces
{
    public interface IDataProvider<TEntity, TRawEntity>
        where TEntity : class ,new()
        where TRawEntity : class
    {
        
        TEntity Get(string objectId);
        
        void Save(TEntity item);
        void SaveMany(IEnumerable<TEntity> items);

        bool IsExisted(TEntity item);

        TRawEntity Convert(TEntity item);
        TEntity Convert(TRawEntity item);
    }
}
