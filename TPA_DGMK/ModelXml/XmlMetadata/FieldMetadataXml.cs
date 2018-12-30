using Data.DataMetadata;
using Data.Enums;
using System.Runtime.Serialization;

namespace ModelXml.XmlMetadata
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(TypeMetadataXml))]
    public class FieldMetadataXml : FieldMetadataBase
    {
        [DataMember] public override string Name { get; set; }
        [DataMember] public new TypeMetadataXml TypeMetadata { get; set; }
        [DataMember] public override AccessLevel AccessLevel { get; set; }
        [DataMember] public override bool IsStatic { get; set; }
    }
}
