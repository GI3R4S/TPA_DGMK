using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoggerDatabase
{
    [Table("Logs")]
    public class DatabaseLogs
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        public string Severity { get; set; }
        public virtual DateTime Time { get; set; }
    }
}
