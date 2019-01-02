using Data.DataMetadata;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ModelDB.Entities
{
    [Export(typeof(AssemblyMetadataBase))]
    public class AssemblyMetadataDB : AssemblyMetadataBase
    {
        public int Id { get; set; }
        public override string Name { get; set; }
        public new List<NamespaceMetadataDB> Namespaces { get; set; }
    }
}
