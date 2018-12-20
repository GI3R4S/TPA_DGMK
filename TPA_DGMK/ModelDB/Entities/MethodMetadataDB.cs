using Data.DataMetadata;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelDB.Entities
{
    [Table("MethodMetadata")]
    public class MethodMetadataDB : MethodMetadataBase
    {
        public MethodMetadataDB()
        {
            GenericArguments = new List<TypeMetadataDB>();
            Parameters = new List<ParameterMetadataDB>();
            TypeConstructors = new HashSet<TypeMetadataDB>();
            TypeMethods = new HashSet<TypeMetadataDB>();
        }

        public int Id { get; set; }
        public override string Name { get; set; }
        public new List<TypeMetadataDB> GenericArguments { get; set; }
        public override Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; set; }
        public new TypeMetadataDB ReturnType { get; set; }
        public override bool Extension { get; set; }
        public new List<ParameterMetadataDB> Parameters { get; set; }

        public virtual ICollection<TypeMetadataDB> TypeConstructors { get; set; }

        public virtual ICollection<TypeMetadataDB> TypeMethods { get; set; }
    }
}
