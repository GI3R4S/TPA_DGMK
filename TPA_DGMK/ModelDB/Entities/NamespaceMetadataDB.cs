using Data.DataMetadata;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelDB.Entities
{
    [Table("NamespaceMetadata")]
    public class NamespaceMetadataDB : NamespaceMetadataBase
    {
        public int Id { get; set; }
        public override string NamespaceName { get; set; }
        public new List<TypeMetadataDB> Types { get; set; }
    }
}
