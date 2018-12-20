using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Data.DataMetadata
{
    [DataContract(IsReference = true)]
    public abstract class AssemblyMetadataBase
    {
        [DataMember] public virtual string Name { get;  set; }
        public virtual List<NamespaceMetadataBase> Namespaces { get; set; }
    }
}
