using GoldenTicket.Model;
using System.Collections.Generic;

namespace GoldenTicket.Data.Interfaces
{
    public interface IUserRequestDataProvider<TRawEntity> : IDataProvider<UserRequest, TRawEntity>
         where TRawEntity : class
    {
        IEnumerable<UserRequest> GetActiveRequests();
    }
}
