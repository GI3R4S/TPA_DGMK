using System.Runtime.Serialization;

namespace Data.DataMetadata
{
    [DataContract(IsReference = true)]
    public abstract class PropertyMetadataBase
    {
        [DataMember] public virtual string Name { get; set; }
        public virtual TypeMetadataBase TypeMetadata { get; set; }
    }
}
