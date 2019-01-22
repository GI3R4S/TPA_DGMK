using System.Data.Entity;
using System.Data.SqlClient;

namespace ModelDB
{
    class DatabaseLoggerContext : DbContext
    {
        public DbSet<DatabaseLogs> Logs { get; set; }

        public DatabaseLoggerContext() : base(GetConnectionString())
        { }
        public static string GetConnectionString()
        {
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            path = path.Remove(path.Length - 10);
            return "data source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=" + path
                + "\\DatabaseForLogging.mdf;integrated security=true;persist security info=True;";
        }

        #region CreateConnection
        public void CreateConnection()
        {
            string conString = GetConnectionString();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Logging') BEGIN CREATE DATABASE Logging END ", connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                SqlCommand command2 = new SqlCommand("USE Logging IF NOT EXISTS(SELECT * FROM sys.objects WHERE name = 'Log') BEGIN CREATE TABLE[dbo].[Log]( [Id][int] IDENTITY(1, 1) NOT NULL, [Date][datetime] NOT NULL, [Thread][varchar](255) NOT NULL, [Level][varchar](50) NOT NULL, [Logger][varchar](255) NOT NULL, [Message][varchar](4000) NOT NULL, [Exception][varchar](2000) NULL) END", connection);
                command2.ExecuteNonQuery();
                command.Connection.Close();
            }
        }
        #endregion
    }
}
