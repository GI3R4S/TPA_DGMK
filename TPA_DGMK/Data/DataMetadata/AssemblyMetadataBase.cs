using System.Collections.Generic;

namespace Data.DataMetadata
{
    public abstract class AssemblyMetadataBase
    {
        public virtual string Name { get; set; }
        public virtual List<NamespaceMetadataBase> Namespaces { get; set; }
    }
}
