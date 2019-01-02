using Data.DataMetadata;
using System.Collections.Generic;

namespace ModelDB.Entities
{
    public class NamespaceMetadataDB : NamespaceMetadataBase
    {
        public int Id { get; set; }
        public override string NamespaceName { get; set; }
        public new List<TypeMetadataDB> Types { get; set; }
    }
}
