using System.ServiceProcess;
using System.Threading;
using GoldenTicket.ConfigurationManager;
using GoldenTicket.Model;
using Parse;

namespace GoldenTicket.RuleEngine.Service
{
    public partial class RuleEngineService : ServiceBase
    {
        private Scheduler.Scheduler<UserRecientBlock> _scheduler;

        public RuleEngineService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Initialize the Parse client with your Application ID and .NET Key found on
            ParseClient.Initialize(Config.ApplicationId, Config.DotNetKey);

            int dueTo = Config.CurrentContext.AppSettings.RuleEngineDueTo;
            int period = Config.CurrentContext.AppSettings.RuleEnginePeriod;
            int workerAmounts = Config.CurrentContext.AppSettings.RuleEngineWorkersAmount;

            _scheduler = new Scheduler.Scheduler<UserRecientBlock>(workerAmounts, dueTo, period);
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
