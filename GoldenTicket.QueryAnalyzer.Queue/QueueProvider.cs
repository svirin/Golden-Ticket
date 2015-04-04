using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using GoldenTicket.Queue.Interfaces;
using Parse;

namespace GoldenTicket.QueryAnalyzer.Queue
{
    public class QueueProvider : IQueueProvider<UserRequest>
    {
        public void Enqueue(ConcurrentQueue<UserRequest> queue)
        {
            var dataProvider = DI.Factory.GetInstance<IUserRequestDataProvider<ParseObject>>();

            var requests = dataProvider.GetActiveRequests().ToList();

            Parallel.ForEach(requests,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                queue.Enqueue);
        }
    }
}
