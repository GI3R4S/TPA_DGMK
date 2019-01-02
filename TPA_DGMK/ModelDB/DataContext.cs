using ModelDB.Entities;
using System.Data.Entity;

namespace ModelDB
{
    public class DataContext : DbContext
    {
        public DataContext(string dataBaseName) : base(dataBaseName)
        {
            Configuration.LazyLoadingEnabled = true;
        }
        public virtual DbSet<AssemblyMetadataDB> AssemblyModel { get; set; }
        public virtual DbSet<NamespaceMetadataDB> Namespaces { get; set; }
        public virtual DbSet<TypeMetadataDB> Types { get; set; }
        public virtual DbSet<MethodMetadataDB> Methods { get; set; }
        public virtual DbSet<PropertyMetadataDB> Properties { get; set; }
        public virtual DbSet<ParameterMetadataDB> Parameters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TypeMetadataDB>().HasMany(t => t.Constructors).WithMany();
            modelBuilder.Entity<TypeMetadataDB>().HasMany(t => t.Fields).WithMany();
            modelBuilder.Entity<TypeMetadataDB>().HasMany(t => t.GenericArguments).WithMany();
            modelBuilder.Entity<TypeMetadataDB>().HasMany(t => t.ImplementedInterfaces).WithMany();
            modelBuilder.Entity<TypeMetadataDB>().HasMany(t => t.Methods).WithMany();
            modelBuilder.Entity<TypeMetadataDB>().HasMany(t => t.NestedTypes).WithMany();
            modelBuilder.Entity<TypeMetadataDB>().HasMany(t => t.Properties).WithMany();
            modelBuilder.Entity<TypeMetadataDB>().HasOptional(t => t.BaseType).WithMany();
            modelBuilder.Entity<TypeMetadataDB>().HasOptional(t => t.DeclaringType).WithMany();
            modelBuilder.Entity<TypeMetadataDB>().HasOptional(t => t.Modifiers);
            modelBuilder.Entity<MethodMetadataDB>().HasMany(m => m.GenericArguments).WithMany();
            modelBuilder.Entity<MethodMetadataDB>().HasMany(m => m.Parameters).WithMany();
            modelBuilder.Entity<MethodMetadataDB>().HasOptional(m => m.Modifiers);
            modelBuilder.Entity<NamespaceMetadataDB>().HasMany(n => n.Types).WithMany();
        }
    }
}
