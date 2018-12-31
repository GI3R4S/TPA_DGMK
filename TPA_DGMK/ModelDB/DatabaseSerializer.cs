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
        public AssemblyMetadataBase Deserialize(string databaseName)
        {
            using (DataContext dataContext = new DataContext(databaseName))
            {
                dataContext.Configuration.ProxyCreationEnabled = false;
                dataContext.Namespaces
                    .Include(n => n.Types).Load();
                dataContext.Types
                    .Include(t => t.Constructors)
                    .Include(t => t.Methods)
                    .Include(t => t.Modifiers)
                    .Include(t => t.BaseType)
                    .Include(t => t.DeclaringType)
                    .Include(t => t.Fields)
                    .Include(t => t.Properties)
                    .Include(t => t.GenericArguments)
                    .Include(t => t.ImplementedInterfaces)
                    .Include(t => t.MethodGenericArguments)
                    .Include(t => t.NestedTypes)
                    .Include(t => t.TypeGenericArguments)
                    .Include(t => t.TypeBaseTypes)
                    .Include(t => t.TypeDeclaringTypes)
                    .Include(t => t.TypeImplementedInterfaces)
                    .Include(t => t.TypeNestedTypes).Load();
                dataContext.Parameters
                    .Include(p => p.TypeMetadata)
                    .Include(p => p.TypeFields)
                    .Include(p => p.MethodParameters).Load();
                dataContext.Methods
                    .Include(m => m.GenericArguments)
                    .Include(m => m.Parameters)
                    .Include(m => m.ReturnType)
                    .Include(m => m.TypeConstructors)
                    .Include(m => m.TypeMethods).Load();
                dataContext.Properties
                    .Include(p => p.TypeMetadata)
                    .Include(p => p.TypeProperties).Load();

                AssemblyMetadataBase assembly = dataContext.AssemblyModel.Include(a => a.Namespaces).ToList().FirstOrDefault();
                return assembly;
            }
        }

        public void Serialize(AssemblyMetadataBase data, string databaseName)
        {
            using (DataContext dataContext = new DataContext(databaseName))
            {
                AssemblyMetadataDB assembly = (AssemblyMetadataDB)data;
                dataContext.AssemblyModel.Add(assembly);
                dataContext.SaveChanges();
            }
        }
    }
}
