using LoggerBase;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Diagnostics;

namespace LoggerToFile
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

        public override void Write(SeverityEnum severity, string message)
        {
            if (severity >= loggingSeverity)
            {
                switch (severity)
                {
                    case SeverityEnum.Information:
                        TraceInformation(message);
                        break;
                    case SeverityEnum.Warning:
                        TraceWarning(message);
                        break;
                    case SeverityEnum.Error:
                        TraceError(message);
                        break;
                    case SeverityEnum.Critical:
                        TraceCritical(message);
                        break;
                }
            }
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