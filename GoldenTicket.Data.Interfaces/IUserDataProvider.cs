using GoldenTicket.Model;
using System.Collections.Generic;

namespace GoldenTicket.Data.Interfaces
{
    public interface IUserDataProvider<TRawEntity> : IDataProvider<User, TRawEntity>
         where TRawEntity : class
    {
        IEnumerable<User> GetAcctualUsers();
    }
}
