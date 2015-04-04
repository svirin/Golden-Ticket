using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using GoldenTicket.Command.Interfaces;
using GoldenTicket.Logger.Log4Net;
using GoldenTicket.Queue.Interfaces;

namespace GoldenTicket.Scheduler
{
    public class Scheduler<TEntity>
        where TEntity : class ,new()
    {
        // Declare timer
        private readonly Timer _tmrTicker;
        private readonly IQueueProvider<TEntity> _queueProvider;
        private readonly ICommandFactory<TEntity> _commandFactory;
        private readonly ConcurrentQueue<TEntity> _bufferQueue;
        private readonly List<Worker<TEntity>> _workers;

        /// <summary>
        /// Scheduler constructor. 
        /// Implements strategy design pattern.
        /// </summary>
        public Scheduler()
        {
            // Declare providers
            _queueProvider = DI.Factory.GetInstance<IQueueProvider<TEntity>>();
            _commandFactory = DI.Factory.GetInstance<ICommandFactory<TEntity>>();

            _workers = new List<Worker<TEntity>>();
            _bufferQueue = new ConcurrentQueue<TEntity>();
            
            _tmrTicker = new Timer(state => TimerTick(), null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Start process
        /// </summary>
        public void Start()
        {
            _tmrTicker.Change(60000, 1000);

            for (var i = 0; i < 5; i++)
            {
                var worker = new Worker<TEntity>(_commandFactory, _bufferQueue);
                
                worker.Start();

                _workers.Add(worker);
            }
        }

        private void TimerTick()
        {
            if (_bufferQueue.Count == 0)
            {
                // Stop timer
                _tmrTicker.Change(Timeout.Infinite, Timeout.Infinite);

                try
                {
                    _queueProvider.Enqueue(_bufferQueue);
                }
                catch (Exception exp)
                {
                    LogFactory.Log.Error(exp);
                }
                finally
                {
                    // Resume timer
                    _tmrTicker.Change(60000, 1000);
                }
            }
        }

        public void Stop()
        {
            _tmrTicker.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}
