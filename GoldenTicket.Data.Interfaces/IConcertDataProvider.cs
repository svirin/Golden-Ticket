using System.Collections.Generic;
using GoldenTicket.Model;

namespace GoldenTicket.Data.Interfaces
{
    public interface IConcertDataProvider<TRawEntity> : IDataProvider<Concert, TRawEntity>
         where TRawEntity : class
    {
        IEnumerable<Concert> GetAll();
        IEnumerable<Concert> GetByUserRequest(Request request);
        IEnumerable<Concert> GetSuggestToUser(User user);
        void SaveMany(IEnumerable<Concert> items);
        void Save(Concert item);
        bool IsExisted(Concert item);
    }
}
