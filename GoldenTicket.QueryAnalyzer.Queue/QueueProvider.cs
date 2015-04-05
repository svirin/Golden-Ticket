using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using GoldenTicket.Queue.Interfaces;
using Parse;

namespace GoldenTicket.QueryAnalyzer.Queue
{
    public class QueueProvider : IQueueProvider<Request>
    {
        public void Enqueue(ConcurrentQueue<Request> queue)
        {
            var dataProvider = DI.Factory.GetInstance<IRequestDataProvider<ParseObject>>();

            var requests = dataProvider.GetActiveRequests().ToList();

            Parallel.ForEach(requests,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                queue.Enqueue);
        }
    }
}
