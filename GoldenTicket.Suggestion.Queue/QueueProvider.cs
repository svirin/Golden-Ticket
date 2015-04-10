using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GoldenTicket.ConfigurationManager;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using GoldenTicket.Queue.Interfaces;
using Parse;

namespace GoldenTicket.Suggestion.Queue
{
    public class QueueProvider : IQueueProvider<User>
    {
        public void Enqueue(ConcurrentQueue<User> queue)
        {
            var dataProvider = DI.Factory.GetInstance<IUserDataProvider<ParseObject>>();
            var users = dataProvider.GetAcctualUsers().ToList();
            int maxParallelTasks = Config.Settings.SuggestionQueueMaxParallelism;

            Parallel.ForEach(users,
                new ParallelOptions { MaxDegreeOfParallelism = maxParallelTasks },
                queue.Enqueue);
        }
    }
}
