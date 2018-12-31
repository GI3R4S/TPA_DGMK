using Data.DataMetadata;
using System.Runtime.Serialization;

namespace ModelXml.XmlMetadata
{
    [DataContract(IsReference = true)]
    public class ParameterMetadataXml : ParameterMetadataBase
    {
        [DataMember] public override string Name { get; set; }
        [DataMember] public new TypeMetadataXml TypeMetadata { get; set; }
    }
}
