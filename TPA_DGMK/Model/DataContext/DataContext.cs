using System.Data.Entity;

namespace Model
{
    public class DataContext : DbContext
    {
        public DataContext(string dataBaseName) : base(dataBaseName)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public virtual DbSet<AssemblyMetadata> AssemblyModel { get; set; }
        public virtual DbSet<NamespaceMetadata> Namespaces { get; set; }
        public virtual DbSet<TypeMetadata> Types { get; set; }
        public virtual DbSet<FieldMetadata> Fields { get; set; }
        public virtual DbSet<MethodMetadata> Methods { get; set; }
        public virtual DbSet<PropertyMetadata> Properties { get; set; }
        public virtual DbSet<ParameterMetadata> Parameters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TypeMetadata>().HasMany(t => t.ImplementedInterfaces).WithMany().Map(cs => cs.ToTable("TypeMetadataImplementedInterfaces"));
            modelBuilder.Entity<TypeMetadata>().HasMany(t => t.GenericArguments).WithMany().Map(cs => cs.ToTable("TypeMetadataGenericArguments"));
            modelBuilder.Entity<TypeMetadata>().HasMany(t => t.Attributes).WithMany().Map(cs => cs.ToTable("TypeMetadataAttributes"));
            modelBuilder.Entity<TypeMetadata>().HasMany(t => t.NestedTypes).WithMany().Map(cs => cs.ToTable("TypeMetadataNestedTypes"));
            modelBuilder.Entity<TypeMetadata>().HasMany(t => t.Constructors).WithMany().Map(cs => cs.ToTable("TypeMetadataConstructors"));
            modelBuilder.Entity<TypeMetadata>().HasOptional(t => t.BaseType).WithMany();
            modelBuilder.Entity<TypeMetadata>().HasOptional(t => t.DeclaringType).WithMany();
            modelBuilder.Entity<MethodMetadata>().HasMany(m => m.AttributesMetadata).WithMany(c => c.Methods).Map(cs => cs.ToTable("MethodMetadataAttributes"));
            modelBuilder.Entity<MethodMetadata>().HasMany(m => m.GenericArguments).WithMany(c => c.Methods2).Map(cs => cs.ToTable("MethodMetadataGenericArguments"));
            modelBuilder.Entity<FieldMetadata>().HasMany(m => m.AttributesMetadata).WithMany(c => c.Fields).Map(cs => cs.ToTable("FieldMetadataAttributes"));
            modelBuilder.Entity<ParameterMetadata>().HasMany(m => m.Attributes).WithMany();
            modelBuilder.Entity<PropertyMetadata>().HasMany(m => m.AttributesMetadata).WithMany(c => c.Properties).Map(cs => cs.ToTable("PropertyMetadataAttributes"));
        }
    }
}
