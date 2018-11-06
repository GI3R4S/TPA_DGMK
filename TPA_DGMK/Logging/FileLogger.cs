using System.Configuration;
using System.Diagnostics;

namespace Logging
{
    public class FileLogger : Logger
    {
        TraceSource log;
        public FileLogger()
        {
            string name = ConfigurationManager.AppSettings["logSourceName"];
            log = new TraceSource(name);
        }
        protected override void TraceInformation(string message)
        {
            log.TraceInformation(message);
        }
        protected override void TraceWarning(string message)
        {
            log.TraceEvent(TraceEventType.Warning, 0, message);
        }
        protected override void TraceError(string message)
        {
            log.TraceEvent(TraceEventType.Error, 0, message);
        }
        protected override void TraceCritical(string message)
        {
            log.TraceEvent(TraceEventType.Critical, 0, message);
        }
    }
}
