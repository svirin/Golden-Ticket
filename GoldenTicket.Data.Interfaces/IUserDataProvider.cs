using GoldenTicket.Model;
using System.Collections.Generic;

namespace GoldenTicket.Data.Interfaces
{
    public interface IUserDataProvider<TRawEntity> : IDataProvider<User, TRawEntity>
         where TRawEntity : class
    {
        IEnumerable<User> GetAcctualUsers();

        void Save(User item);
        void SaveMany(IEnumerable<User> items);

        void Delete(User item);
        void DeleteMany(IEnumerable<User> items);
    }
}
