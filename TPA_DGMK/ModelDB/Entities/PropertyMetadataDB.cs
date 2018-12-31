using Data.DataMetadata;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelDB.Entities
{
    [Table("PropertyMetadata")]
    public class PropertyMetadataDB : PropertyMetadataBase
    {
        public PropertyMetadataDB()
        {
            TypeProperties = new HashSet<TypeMetadataDB>();
        }
        public int Id { get; set; }
        [Required]
        public override string Name { get; set; }
        public new TypeMetadataDB TypeMetadata { get; set; }
        public virtual ICollection<TypeMetadataDB> TypeProperties { get; set; }
    }
}
