using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class NamespaceMetadata
    {
        private string m_NamespaceName;
        private IEnumerable<TypeMetadata> m_Types;

        public string NamespaceName { get => m_NamespaceName; set => m_NamespaceName = value; }
        public IEnumerable<TypeMetadata> Types { get => m_Types; set => m_Types = value; }

        internal NamespaceMetadata(string name, IEnumerable<Type> types)
        {
            NamespaceName = name;
            Types = (from type in types orderby type.Name select new TypeMetadata(type)).ToList();
        }
    }
}
