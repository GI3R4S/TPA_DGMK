using Data.DataMetadata;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelXml.XmlMetadata
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(TypeMetadataXml))]
    public class NamespaceMetadataXml : NamespaceMetadataBase
    {
        [DataMember] public override string NamespaceName { get; set; }
        [DataMember] public new List<TypeMetadataXml> Types { get; set; }
    }
}
