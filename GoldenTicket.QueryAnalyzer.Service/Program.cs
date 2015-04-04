using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GoldenTicket.QueryAnalyzer.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // Initialize windows service
            var service = new AnalyzerService();

            // If in debugger
            if (args.Length > 0 && args[0] == "RUNINDEBUGGER")
            {
                // Run service as debugged
                service.Run();
            }
            else
            {
                // Run service as released
                var servicesToRun = new ServiceBase[] { service };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
