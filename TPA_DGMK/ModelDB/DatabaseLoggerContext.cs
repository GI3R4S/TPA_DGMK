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
    }
}
