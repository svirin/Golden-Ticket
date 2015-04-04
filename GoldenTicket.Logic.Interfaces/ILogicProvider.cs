using System.Collections.Generic;

namespace GoldenTicket.Logic.Interfaces
{
    public interface ILogicProvider<in TRequest, out TResponse>
        where TRequest : class ,new()
        where TResponse : class ,new()
    {
        IEnumerable<TResponse> CrawleByRequest(TRequest requestItem);
    }
}
