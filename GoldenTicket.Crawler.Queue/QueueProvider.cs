using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GoldenTicket.Data.Interfaces;
using GoldenTicket.Model;
using GoldenTicket.Queue.Interfaces;
using Parse;

namespace GoldenTicket.Crawler.Queue
{
    public class QueueProvider : IQueueProvider<Artist>
    {
        public void Enqueue(ConcurrentQueue<Artist> queue)
        {
            var dataProvider = DI.Factory.GetInstance<IArtistDataProvider<ParseObject>>();

            var artists = dataProvider.GetAcctualArtists().ToList();

            Parallel.ForEach(artists,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                queue.Enqueue);
        }
    }
}
