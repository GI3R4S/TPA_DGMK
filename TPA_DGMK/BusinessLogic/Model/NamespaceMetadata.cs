using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Model
{
    public class NamespaceMetadata
    {
        public string NamespaceName { get; set; }
        public List<TypeMetadata> Types { get; set; }

        internal NamespaceMetadata(string name, IList<Type> types)
        {
            NamespaceName = name;
            Types = (from type in types orderby type.Name select new TypeMetadata(type)).ToList();
        }

        public NamespaceMetadata() { }
    }
}
