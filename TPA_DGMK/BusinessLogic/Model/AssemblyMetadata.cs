using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Model
{
    public class AssemblyMetadata
    {
        public AssemblyMetadata(Assembly assembly)
        {
            Name = assembly.FullName;
            Namespaces = (from Type _type in assembly.GetTypes()
                           where _type.GetVisible()
                           group _type by _type.GetNamespace() into _group
                           orderby _group.Key
                           select new NamespaceMetadata(_group.Key, _group)).ToList();
        }
        private string m_Name;
        private IEnumerable<NamespaceMetadata> m_Namespaces;

        public IEnumerable<NamespaceMetadata> Namespaces { get => m_Namespaces; private set => m_Namespaces = value; }
        public string Name { get => m_Name; private set => m_Name = value; }
    }
}
