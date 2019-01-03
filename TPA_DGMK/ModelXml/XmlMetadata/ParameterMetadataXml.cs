using Data.DataMetadata;

namespace ModelXml.XmlMetadata
{
    public class ParameterMetadataXml : ParameterMetadataBase
    {
        public override string Name { get; set; }
        public new TypeMetadataXml TypeMetadata { get; set; }
    }
}
