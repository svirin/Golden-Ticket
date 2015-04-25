using System.Threading.Tasks;
using GoldenTicket.ConfigurationManager;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using GoldenTicket.Queue.Interfaces;
using Parse;
using System.Collections.Concurrent;
using System.Linq;

namespace GoldenTicket.Suggestion.Queue
{
     public class QueueProvider : IQueueProvider<User>
    {
        public void Enqueue(ConcurrentQueue<User> queue)
        {
            var dataProvider = DI.Factory.GetInstance<IUserDataProvider<ParseObject>>();

            var artists = dataProvider.GetAcctualUsers().ToList();

            int maxParallelTasks = Config.CurrentContext.AppSettings.SuggestionQueueMaxParallelism;

            Parallel.ForEach(artists,
                new ParallelOptions { MaxDegreeOfParallelism = maxParallelTasks },
                queue.Enqueue);
        }
    }
}
