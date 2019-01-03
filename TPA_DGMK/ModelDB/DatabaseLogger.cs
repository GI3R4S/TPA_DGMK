using LoggerBase;
using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data.SqlClient;

namespace ModelDB
{
    [Export(typeof(Logger))]
    public class DatabaseLogger : Logger
    {
        private log4net.ILog log;
        public DatabaseLogger(Type type)
        {
            string conString = "Data source=.;integrated security=true;persist security info=True;";
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Logging') BEGIN CREATE DATABASE Logging END ", connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                SqlCommand command2 = new SqlCommand("USE Logging IF NOT EXISTS(SELECT * FROM sys.objects WHERE name = 'Log') BEGIN CREATE TABLE[dbo].[Log]( [Id][int] IDENTITY(1, 1) NOT NULL, [Date][datetime] NOT NULL, [Thread][varchar](255) NOT NULL, [Level][varchar](50) NOT NULL, [Logger][varchar](255) NOT NULL, [Message][varchar](4000) NOT NULL, [Exception][varchar](2000) NULL) END", connection);
                command2.ExecuteNonQuery();
                command.Connection.Close();
            }
            log4net.Config.XmlConfigurator.Configure();
            log = log4net.LogManager.GetLogger(type);
        }
        public DatabaseLogger()
        {
            string conString = "Data source=.;integrated security=true;persist security info=True;";
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Logging') BEGIN CREATE DATABASE Logging END ", connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                SqlCommand command2 = new SqlCommand("USE Logging IF NOT EXISTS(SELECT * FROM sys.objects WHERE name = 'Log') BEGIN CREATE TABLE[dbo].[Log]( [Id][int] IDENTITY(1, 1) NOT NULL, [Date][datetime] NOT NULL, [Thread][varchar](255) NOT NULL, [Level][varchar](50) NOT NULL, [Logger][varchar](255) NOT NULL, [Message][varchar](4000) NOT NULL, [Exception][varchar](2000) NULL) END", connection);
                command2.ExecuteNonQuery();
                command.Connection.Close();
            }
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
