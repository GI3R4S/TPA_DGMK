using System.Collections.Generic;

namespace Data.DataMetadata
{
    public abstract class NamespaceMetadataBase
    {
        public virtual string NamespaceName { get; set; }
        public virtual List<TypeMetadataBase> Types { get; set; }
    }
}
