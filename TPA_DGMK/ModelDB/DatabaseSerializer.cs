using System.Data.Entity;
using System.Linq;
using System.ComponentModel.Composition;
using Data;
using Data.DataMetadata;
using ModelDB.Entities;

namespace ModelDB
{
    [Export(typeof(ISerializer))]
    public class DatabaseSerializer : ISerializer
    {
        public void Serialize(AssemblyMetadataBase data, string databaseName)
        {
            Database.Delete(databaseName);
            using (DataContext dataContext = new DataContext(databaseName))
            {
                AssemblyMetadataDB assembly = (AssemblyMetadataDB)data;
                dataContext.AssemblyModel.Add(assembly);
                dataContext.SaveChanges();
            }
        }

        public AssemblyMetadataBase Deserialize(string databaseName)
        {
            using (DataContext dataContext = new DataContext(databaseName))
            {
                dataContext.Configuration.ProxyCreationEnabled = false;
                AssemblyMetadataBase assembly = dataContext.AssemblyModel.Include(a => a.Namespaces).ToList().FirstOrDefault();
                dataContext.Namespaces
                    .Include(n => n.Types).Load();
                dataContext.Types
                    .Include(t => t.Constructors).Include(t => t.Methods)
                    .Include(t => t.BaseType).Include(t => t.DeclaringType)
                    .Include(t => t.Fields).Include(t => t.Properties)
                    .Include(t => t.GenericArguments).Include(t => t.ImplementedInterfaces)
                    .Include(t => t.Modifiers).Include(t => t.NestedTypes).Load();
                dataContext.Methods
                    .Include(m => m.GenericArguments).Include(m => m.Parameters)
                    .Include(m => m.Modifiers).Include(m => m.ReturnType).Load();
                dataContext.Parameters
                    .Include(p => p.TypeMetadata).Load();
                dataContext.Properties
                    .Include(p => p.TypeMetadata).Load();
                return assembly;
            }
        }
    }
}
