using System;
using System.Configuration;

namespace Logging
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
        public void Write(SeverityEnum severity, string message)
        {
            if (severity >= loggingSeverity)
            {
                switch (severity)
                {
                    case SeverityEnum.Information:
                        this.TraceInformation(message);
                        break;
                    case SeverityEnum.Warning:
                        this.TraceWarning(message);
                        break;
                    case SeverityEnum.Error:
                        this.TraceError(message);
                        break;
                    case SeverityEnum.Critical:
                        this.TraceCritical(message);
                        break;
                }
            }
        }
        protected abstract void TraceInformation(string message);
        protected abstract void TraceWarning(string message);
        protected abstract void TraceError(string message);
        protected abstract void TraceCritical(string message);
    }
}
