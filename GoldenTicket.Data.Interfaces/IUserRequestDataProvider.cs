using GoldenTicket.Model;
using System.Collections.Generic;

namespace GoldenTicket.Data.Interfaces
{
    public interface IRequestDataProvider<TRawEntity> : IDataProvider<Request, TRawEntity>
         where TRawEntity : class
    {
        IEnumerable<Request> GetActivatedRequests();
        void ActivateRequest(Request item);
    }
}
