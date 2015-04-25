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
        private readonly int _period;
        private readonly int _dueTime;
        private readonly int _workersAmount;

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
        public Scheduler(int workersAmount, int dueTime, int period)
        {
            _period = period;
            _dueTime = dueTime;
            _workersAmount = workersAmount;

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
            _tmrTicker.Change(_dueTime, _period);

            for (var workerIndex = 0; workerIndex < _workersAmount; workerIndex++)
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
                    _tmrTicker.Change(_dueTime, _period);
                }
            }
        }

        public void Stop()
        {
            _tmrTicker.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}
