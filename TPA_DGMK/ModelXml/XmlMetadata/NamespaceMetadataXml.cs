using Data.DataMetadata;
using System.Collections.Generic;

namespace ModelXml.XmlMetadata
{
    public class NamespaceMetadataXml : NamespaceMetadataBase
    {
        public override string NamespaceName { get; set; }
        public new List<TypeMetadataXml> Types { get; set; }
    }
}
