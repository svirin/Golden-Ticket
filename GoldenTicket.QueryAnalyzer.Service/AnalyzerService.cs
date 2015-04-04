using GoldenTicket.Model;
using Parse;
using System.ServiceProcess;
using System.Threading;

namespace GoldenTicket.QueryAnalyzer.Service
{
    /// <summary>
    /// Service algorythm:
    /// 1. Load all user requests (not signed as finished) and queued them
    /// 2. Each worker dequeue user request.
    /// 3. Try to load artist by user request.
    /// 4. If artist not existed -> create him
    /// 5. Sign user request as finished
    /// </summary>
    public partial class AnalyzerService : ServiceBase
    {
        private Scheduler.Scheduler<UserRequest> _scheduler;

        public AnalyzerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Initialize the Parse client with your Application ID and .NET Key found on
            ParseClient.Initialize("g6S8spz8mWaRmettl8puz8sMDkgL6enP8pJJ4oTK", "T5gykzQsG1KavfYBlC42iswiopIwcfHEgvTBCXfl");

            _scheduler = new Scheduler.Scheduler<UserRequest>();

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
