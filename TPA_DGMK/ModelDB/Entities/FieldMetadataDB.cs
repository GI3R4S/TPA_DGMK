using Data.DataMetadata;
using Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelDB.Entities
{
    [Table("FieldMetadata")]
    public class FieldMetadataDB : FieldMetadataBase
    {
        public int Id { get; set; }
        public override string Name { get; set; }
        public new TypeMetadataDB TypeMetadata { get; set; }
        public new List<TypeMetadataDB> AttributesMetadata { get; set; }
        public override AccessLevel AccessLevel { get; set; }
        public override bool IsStatic { get; set; }
    }
}
