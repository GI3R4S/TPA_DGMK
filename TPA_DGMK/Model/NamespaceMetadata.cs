using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Model
{
    [DataContract(IsReference = true)]
    public class NamespaceMetadata
    {
        [DataMember]
        public string NamespaceName{get; private set;}
        [DataMember]
        public IEnumerable<TypeMetadata> Types{get; private set;}

        internal NamespaceMetadata(string name, IEnumerable<Type> types)
        {
            NamespaceName = name;
            Types = (from type in types orderby type.Name select new TypeMetadata(type)).ToList();
        }

        private NamespaceMetadata() { }
    }
}
