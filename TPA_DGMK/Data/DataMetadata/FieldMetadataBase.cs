using Data.Enums;
using System.Runtime.Serialization;

namespace Data.DataMetadata
{
    [DataContract(IsReference = true)]
    public abstract class FieldMetadataBase
    {
        [DataMember] public virtual string Name { get; set; }
        public virtual TypeMetadataBase TypeMetadata { get; set; }
        public virtual AccessLevel AccessLevel { get; set; }
        [DataMember] public virtual bool IsStatic { get; set; }
    }
}
