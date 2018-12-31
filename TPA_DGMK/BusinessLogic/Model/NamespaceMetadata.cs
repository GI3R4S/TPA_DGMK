using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Model
{
    public class NamespaceMetadata
    {
        public string NamespaceName { get; set; }
        public List<TypeMetadata> Types { get; set; }

        public NamespaceMetadata(string name, IList<Type> types)
        {
            NamespaceName = name;
            Types = types.OrderBy(t => t.Name).Select(TypeMetadata.EmitType).ToList();
        }

        public NamespaceMetadata() { }
    }
}
