using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoldenTicket.Watchdog
{
    public partial class WatchdogService : ServiceBase
    {
        private Timer _tmrTicker;

        public WatchdogService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _tmrTicker = new Timer(TickEvent, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        private void TickEvent(object state)
        {
            try
            {
                _tmrTicker.Change(Timeout.Infinite, Timeout.Infinite);

                var service = GetService("service name");

                var controller = new ServiceController(service.Name);

                if (controller.Status != ServiceControllerStatus.Running)
                {
                    controller.Start();
                }
            }
            catch (Exception exp)
            {
                // Log
            }
            finally
            {
                _tmrTicker.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            }
        }

        protected override void OnStop()
        {
            _tmrTicker.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private ServiceDetails GetService(string serviceName)
        {
            var filter = string.Format("SELECT * FROM Win32_Service WHERE Name = '{0}'", serviceName);
            var query = new ManagementObjectSearcher(filter);
            var services = query.Get();

            if (services.Count < 0)
                throw new ManagementException(string.Format("Service {0} not found. Probably it not installed", serviceName));
            if (services.Count > 1)
                throw new ManagementException(string.Format("Service {0} is duplicated", serviceName));

            var serviceDetails = (from ManagementObject service in services
                                  let name = service.GetPropertyValue("Name").ToString()
                                  let pid = (uint)service.GetPropertyValue("ProcessId")
                                  where !string.IsNullOrEmpty(name)
                                  select new ServiceDetails { Name = name, Pid = pid }).Single();
            
            return serviceDetails;
        }
    }

    public class ServiceDetails
    {
        public string Name { get; set; }

        public uint Pid { get; set; }
    }
}
