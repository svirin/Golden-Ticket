using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GoldenTicket.ConfigurationManager;
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
            int maxParallelTasks = Config.Settings.CrawlerQueueMaxParallelism;

            Parallel.ForEach(artists,
                new ParallelOptions { MaxDegreeOfParallelism = maxParallelTasks },
                queue.Enqueue);
        }
    }
}
