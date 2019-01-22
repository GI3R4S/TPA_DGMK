using System;
using System.Configuration;

namespace LoggerBase
{
    public abstract class Logger
    {
        protected SeverityEnum loggingSeverity;
        public Logger()
        {
            string severity = string.Empty;
            severity = ConfigurationManager.AppSettings["traceLevel"];
            Enum.TryParse(severity, out loggingSeverity);
        }
        public abstract void Write(SeverityEnum severity, string message);
        protected abstract void TraceInformation(string message);
        protected abstract void TraceWarning(string message);
        protected abstract void TraceError(string message);
        protected abstract void TraceCritical(string message);
    }
}
