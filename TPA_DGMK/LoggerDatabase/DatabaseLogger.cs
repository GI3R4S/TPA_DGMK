using LoggerBase;
using System;
using System.ComponentModel.Composition;

namespace LoggerDatabase
{
    [Export(typeof(Logger))]
    public class DatabaseLogger : Logger
    {
        public override void Write(SeverityEnum severity, string message)
        {
            using (DatabaseLoggerContext context = new DatabaseLoggerContext())
            {
                context.Logs.Add(new DatabaseLogs
                {
                    Severity = severity.ToString(),
                    Message = message,
                    Time = DateTime.Now
                });
                context.SaveChanges();
            }
        }
        #region Trace
        protected override void TraceInformation(string message) { }
        protected override void TraceWarning(string message) { }
        protected override void TraceError(string message) { }
        protected override void TraceCritical(string message) { }
        #endregion
    }
}
