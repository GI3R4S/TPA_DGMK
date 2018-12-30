using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Data.DataMetadata
{
    [DataContract(IsReference = true)]
    public abstract class NamespaceMetadataBase
    {
        [DataMember] public virtual string NamespaceName { get; set; }
        public virtual List<TypeMetadataBase> Types { get; set; }
    }
}
