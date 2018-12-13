using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using Model;

namespace Data_De_Serialization
{
    public class DatabaseSerializer : SerializerTemplate
    {
        public DatabaseSerializer()
        { }

        public override T Deserialize<T>(string databaseName)
        {
            AssemblyMetadata assembly;
            using (DataContext dataContext = new DataContext(databaseName))
            {
                dataContext.Fields.Load();
                dataContext.Properties.Load();
                dataContext.Parameters.Load();
                dataContext.Methods.Include(m => m.Parameters).Load();
                dataContext.AssemblyModel.Include(a => a.Namespaces).Load();
                dataContext.Namespaces.Include(n => n.Types).Load();
                dataContext.Types.Include(t => t.Methods).Include(t => t.Methods2).
                    Include(t => t.ImplementedInterfaces).Include(t => t.NestedTypes).
                    Include(t => t.Properties).Include(t => t.Attributes).Include(t => t.BaseType).
                    Include(t => t.Constructors).Include(t => t.DeclaringType).Include(t => t.Fields).
                    Include(t => t.GenericArguments).Load();
                assembly = dataContext.AssemblyModel.First();
            }
            try
            {
                return (T)(object)assembly;
            }
            catch
            {
                return (T)Activator.CreateInstance(typeof(T), assembly);
            }
        }

        public override void Serialize<T>(T data, string databaseName)
        {
            using (DataContext dataContext = new DataContext(databaseName))
            {
                AssemblyMetadata assembly = (AssemblyMetadata)Convert.ChangeType(data, typeof(AssemblyMetadata));
                dataContext.AssemblyModel.Add(assembly);
                dataContext.SaveChanges();
            }
            DialogResult result = MessageBox.Show("Done", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }
    }
}
