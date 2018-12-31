using Data.DataMetadata;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelDB.Entities
{
    [Table("ParameterMetadata")]
    public class ParameterMetadataDB : ParameterMetadataBase
    {
        public ParameterMetadataDB()
        {
            MethodParameters = new HashSet<MethodMetadataDB>();
            TypeFields = new HashSet<TypeMetadataDB>();
        }
        public int Id { get; set; }
        [Required]
        public override string Name { get; set; }
        public new TypeMetadataDB TypeMetadata { get; set; }

        public virtual ICollection<MethodMetadataDB> MethodParameters { get; set; }

        public virtual ICollection<TypeMetadataDB> TypeFields { get; set; }
    }
}
