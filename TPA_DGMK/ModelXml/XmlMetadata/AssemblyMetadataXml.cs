using Data.DataMetadata;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ModelXml.XmlMetadata
{
    [Export(typeof(AssemblyMetadataBase))]
    public class AssemblyMetadataXml : AssemblyMetadataBase
    {
        public override string Name { get; set; }
        public new List<NamespaceMetadataXml> Namespaces { get; set; }
    }
}
