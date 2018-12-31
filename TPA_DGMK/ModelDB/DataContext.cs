using ModelDB.Entities;
using System.Data.Entity;

namespace ModelDB
{
    public class DataContext : DbContext
    {
        public DataContext(string dataBaseName) : base(dataBaseName)
        {
            // this.Configuration.LazyLoadingEnabled = true;
        }
        public virtual DbSet<AssemblyMetadataDB> AssemblyModel { get; set; }
        public virtual DbSet<NamespaceMetadataDB> Namespaces { get; set; }
        public virtual DbSet<TypeMetadataDB> Types { get; set; }
        public virtual DbSet<MethodMetadataDB> Methods { get; set; }
        public virtual DbSet<PropertyMetadataDB> Properties { get; set; }
        public virtual DbSet<ParameterMetadataDB> Parameters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
