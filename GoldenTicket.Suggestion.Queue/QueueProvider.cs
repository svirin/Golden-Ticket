using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
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

            Parallel.ForEach(users,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                queue.Enqueue);
        }
    }
}
