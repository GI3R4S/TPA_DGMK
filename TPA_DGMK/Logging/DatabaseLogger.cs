using System;
using System.Configuration;

namespace Logging
{
    public class DatabaseLogger : Logger
    {
        private log4net.ILog log;
        public DatabaseLogger(Type type)
        {
            log4net.Config.XmlConfigurator.Configure();
            log = log4net.LogManager.GetLogger(type);
        }
        public DatabaseLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
            log = log4net.LogManager.GetLogger(ConfigurationManager.AppSettings["logSourceName"]);
        }
        protected override void TraceInformation(string message)
        {
            log.Info(message);
        }
        protected override void TraceWarning(string message)
        {
            log.Warn(message);
        }
        protected override void TraceError(string message)
        {
            log.Error(message);
        }
        protected override void TraceCritical(string message)
        {
            log.Fatal(message);
        }
    }
}
