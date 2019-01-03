using Data.DataMetadata;

namespace ModelXml.XmlMetadata
{
    public class PropertyMetadataXml : PropertyMetadataBase
    {
        public override string Name { get; set; }
        public new TypeMetadataXml TypeMetadata { get; set; }
    }
}
