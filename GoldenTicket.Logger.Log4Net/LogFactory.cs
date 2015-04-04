using System;
using System.Configuration;
using System.IO;
using log4net;
using log4net.Config;

namespace GoldenTicket.Logger.Log4Net
{
    public class LogFactory
    {
        static LogFactory()
        {
            var directoryPath = Environment.CurrentDirectory;
            var logFilePath = ConfigurationManager.AppSettings["LogConfigPath"] ?? @"Log4net.config";
            var fullPath = Path.Combine(directoryPath, logFilePath);
            var finfo = new FileInfo(fullPath);
            XmlConfigurator.Configure(finfo);
        }

        private static string _logName = "empty";
        public static void Configure(string logName)
        {
            _logName = logName;
        }

        public static ILog Log
        {
            get { return LogManager.GetLogger(_logName); }
        }
    }
}
