using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Model
{
    [DataContract(IsReference = true)]
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

        private AssemblyMetadata() { }

        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public IEnumerable<NamespaceMetadata> Namespaces { get; private set; }
    }
}
