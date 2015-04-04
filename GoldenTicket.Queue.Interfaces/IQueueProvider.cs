using System.Collections.Concurrent;

namespace GoldenTicket.Queue.Interfaces
{
    public interface IQueueProvider<TEntity>
        where TEntity : class, new()
    {
        void Enqueue(ConcurrentQueue<TEntity> queue);
    }
}
