using Data.DataMetadata;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelDB.Entities
{
    [Table("TypeMetadata")]
    public class TypeMetadataDB : TypeMetadataBase
    {
        public TypeMetadataDB()
        {
            MethodGenericArguments = new HashSet<MethodMetadataDB>();
            TypeGenericArguments = new HashSet<TypeMetadataDB>();
            TypeImplementedInterfaces = new HashSet<TypeMetadataDB>();
            TypeNestedTypes = new HashSet<TypeMetadataDB>();
            Constructors = new List<MethodMetadataDB>();
            Fields = new List<FieldMetadataDB>();
            GenericArguments = new List<TypeMetadataDB>();
            ImplementedInterfaces = new List<TypeMetadataDB>();
            Methods = new List<MethodMetadataDB>();
            NestedTypes = new List<TypeMetadataDB>();
            Properties = new List<PropertyMetadataDB>();

        }

        [Key]
        public override string TypeName { get; set; }
        public override string NamespaceName { get; set; }
        public override string FullTypeName { get; set; }
        public new TypeMetadataDB BaseType { get; set; }
        public new TypeMetadataDB DeclaringType { get; set; }
        public new List<TypeMetadataDB> GenericArguments { get; set; }
        public override Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers { get; set; }
        public override TypeKind TypeKind { get; set; }
        public new List<TypeMetadataDB> Attributes { get; set; }
        public new List<TypeMetadataDB> ImplementedInterfaces { get; set; }
        public new List<TypeMetadataDB> NestedTypes { get; set; }
        public new List<PropertyMetadataDB> Properties { get; set; }
        public new List<FieldMetadataDB> Fields { get; set; }
        public new List<MethodMetadataDB> Methods { get; set; }
        public new List<MethodMetadataDB> Constructors { get; set; }
        public override bool IsAbstract { get; set; }
        public override bool IsSealed { get; set; }

        [InverseProperty("BaseType")]
        public virtual ICollection<TypeMetadataDB> TypeBaseTypes { get; set; }

        [InverseProperty("DeclaringType")]
        public virtual ICollection<TypeMetadataDB> TypeDeclaringTypes { get; set; }

        public virtual ICollection<MethodMetadataDB> MethodGenericArguments { get; set; }

        public virtual ICollection<TypeMetadataDB> TypeGenericArguments { get; set; }

        [InverseProperty("ImplementedInterfaces")]
        public virtual ICollection<TypeMetadataDB> TypeImplementedInterfaces { get; set; }

        public virtual ICollection<TypeMetadataDB> TypeNestedTypes { get; set; }

    }
}
