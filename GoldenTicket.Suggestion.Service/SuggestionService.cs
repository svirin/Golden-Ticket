using System.ServiceProcess;
using System.Threading;
using GoldenTicket.Model;

namespace GoldenTicket.Suggestion.Service
{
    public partial class CrawlerServices : ServiceBase
    {
        private Scheduler.Scheduler<User> _scheduler;

        public CrawlerServices()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _scheduler = new Scheduler.Scheduler<User>();

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
