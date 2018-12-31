using BusinessLogic.Model;
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
            Assembly assembly = Assembly.LoadFrom(assemblyFile);
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
