using Model;
using System.Reflection;

namespace ViewModel
{
    public class Reflector
    {
        public AssemblyMetadata AssemblyMetadata { get; private set; }
        public Assembly Assembly { get; private set; }
        public Reflector(string assemblyFile)
        {
            if (string.IsNullOrEmpty(assemblyFile))
                throw new System.ArgumentNullException();
            Assembly = Assembly.LoadFrom(assemblyFile);
            AssemblyMetadata = new AssemblyMetadata(Assembly);
        }
        public Reflector(Assembly assembly)
        {
            AssemblyMetadata = new AssemblyMetadata(assembly);
        }
    }
}
