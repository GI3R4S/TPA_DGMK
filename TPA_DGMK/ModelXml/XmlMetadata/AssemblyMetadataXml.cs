using Data.DataMetadata;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;

namespace ModelXml.XmlMetadata
{
    [DataContract(IsReference = true)]
    [Export(typeof(AssemblyMetadataBase))]
    [KnownType(typeof(NamespaceMetadataXml))]
    public class AssemblyMetadataXml : AssemblyMetadataBase
    {
        [DataMember] public override string Name { get; set; }
        [DataMember] public new List<NamespaceMetadataXml> Namespaces { get; set; }
    }
}
