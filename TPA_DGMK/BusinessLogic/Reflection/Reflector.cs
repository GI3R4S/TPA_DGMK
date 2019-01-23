using BusinessLogic.Model;
using System.IO;
using System.Reflection;

namespace BusinessLogic.Reflection
{
    public class Reflector
    {
        public AssemblyMetadata AssemblyMetadata { get; private set; }
        public Reflector(string assemblyFile)
        {
            if (string.IsNullOrEmpty(assemblyFile))
                throw new System.ArgumentNullException();
            Assembly assembly = Assembly.ReflectionOnlyLoadFrom(assemblyFile);
            foreach (var assemblyName in assembly.GetReferencedAssemblies())
            {
                try
                {
                    Assembly.ReflectionOnlyLoad(assemblyName.FullName);
                }
                catch
                {
                    Assembly.ReflectionOnlyLoadFrom(Path.Combine(Path.GetDirectoryName(assemblyFile), assemblyName.Name + ".dll"));
                }
            }
            AssemblyMetadata = new AssemblyMetadata(assembly);
        }
        public Reflector(Assembly assembly)
        {
            AssemblyMetadata = new AssemblyMetadata(assembly);
        }
        public Reflector(AssemblyMetadata assemblyMetadata)
        {
            AssemblyMetadata = assemblyMetadata;
        }
    }
}
