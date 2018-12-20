using LoggerBase;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Diagnostics;

namespace ModelXml
{
    [Export(typeof(Logger))]
    public class FileLogger : Logger
    {
        TraceSource traceSource;
        public FileLogger()
        {
            string fileName = ConfigurationManager.AppSettings["logSourceName"];
            traceSource = new TraceSource(fileName);
        }
        protected override void TraceInformation(string message)
        {
            traceSource.TraceInformation(message);
        }
        protected override void TraceWarning(string message)
        {
            traceSource.TraceEvent(TraceEventType.Warning, 0, message);
        }
        protected override void TraceError(string message)
        {
            traceSource.TraceEvent(TraceEventType.Error, 0, message);
        }
        protected override void TraceCritical(string message)
        {
            traceSource.TraceEvent(TraceEventType.Critical, 0, message);
        }
    }
}
