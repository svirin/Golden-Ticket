using System.ServiceProcess;
using System.Threading;
using Castle.Core.Resource;
using GoldenTicket.ConfigurationManager;
using GoldenTicket.Model;
using Parse;

namespace GoldenTicket.Crawler.Service
{
    public partial class CrawlerService : ServiceBase
    {
        private Scheduler.Scheduler<Artist> _scheduler;

        public CrawlerService()
        {
            InitializeComponent();
            //LogFactory.Configure("Crawler");
        }

        protected override void OnStart(string[] args)
        {
            //LogFactory.Log.Info("Start crawler service");

            // Initialize the Parse client with your Application ID and .NET Key found on
            ParseClient.Initialize(Config.ApplicationId, Config.DotNetKey);

            int dueTo = Config.CurrentContext.AppSettings.CreawlerDueTo;
            int period = Config.CurrentContext.AppSettings.CreawlerPeriod;
            int workerAmounts = Config.CurrentContext.AppSettings.CreawlerWorkersAmount;

            _scheduler = new Scheduler.Scheduler<Artist>(workerAmounts, dueTo, period);
            _scheduler.Start();
        }

        protected override void OnStop()
        {
            _scheduler.Stop();
        }

        /// <summary> Method for debugger only</summary>
        public void Run()
        {
            // Start service
            OnStart(new string[0]);

            // Initial thread with nonstop operation
            new Thread(() => Thread.Sleep(-1)).Start();
        }
    }
}
