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
        public int Id { get; private set; }
        [DataMember]
        public string NamespaceName{get; private set;}
        [DataMember]
        public ICollection<TypeMetadata> Types{get; private set;}

        internal NamespaceMetadata(string name, IEnumerable<Type> types)
        {
            Id = ++counter;
            NamespaceName = name;
            Types = (from type in types orderby type.Name select new TypeMetadata(type)).ToList();
        }

        private NamespaceMetadata() { }
        private static int counter = 0;
    }
}
