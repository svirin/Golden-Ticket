using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GoldenTicket.ConfigurationManager;
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
            var requests = dataProvider.GetActivatedRequests().ToList();
            int maxParallelTasks = Config.CurrentContext.AppSettings.QueryAnalizerQueueMaxParallelism;

            Parallel.ForEach(requests,
                new ParallelOptions { MaxDegreeOfParallelism = maxParallelTasks },
                queue.Enqueue);
        }
    }
}
