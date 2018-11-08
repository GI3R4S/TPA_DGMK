using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class NamespaceMetadata
    {
        public string NamespaceName{get; private set;}
        public IEnumerable<TypeMetadata> Types{get; private set;}

        internal NamespaceMetadata(string name, IEnumerable<Type> types)
        {
            NamespaceName = name;
            Types = (from type in types orderby type.Name select new TypeMetadata(type)).ToList();
        }
    }
}
