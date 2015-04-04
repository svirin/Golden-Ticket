using System;
using System.Collections.Concurrent;
using System.Threading;
using GoldenTicket.Command.Interfaces;
using GoldenTicket.Logger.Log4Net;

namespace GoldenTicket.Scheduler
{
    public class Worker<TEntity>
        where TEntity : class ,new()
    {
        private readonly ICommandFactory<TEntity> _commandFactory;
        private readonly ConcurrentQueue<TEntity> _bufferQueue;

        public Worker(ICommandFactory<TEntity> commandFactory, ConcurrentQueue<TEntity> bufferQueue)
        {
            _commandFactory = commandFactory;
            _bufferQueue = bufferQueue;
        }

        public void Start()
        {
            var tread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        TEntity item;

                        if (_bufferQueue.TryDequeue(out item))
                        {
                            var command = _commandFactory.CreateCommand();

                            command.ExecuteCommand(item);
                        }
                    }
                    catch (Exception exp)
                    {
                        LogFactory.Log.Error(exp);
                    }
                }
            });
            tread.Start();

            //Task.Factory.StartNew(() =>
            //{
            //    while (true)
            //    {
            //        try
            //        {

            //            TEntity identity;

            //            if (_bufferQueue.TryDequeue(out identity))
            //            {
            //                var command = _commandFactory.CreateCommand();

            //                command.ExecuteCommand(identity);
            //            }
            //        }
            //        catch (Exception exp)
            //        {
            //            LogFactory.Log.Error(exp);
            //        }
            //    }
            //});
        }
    }
}
