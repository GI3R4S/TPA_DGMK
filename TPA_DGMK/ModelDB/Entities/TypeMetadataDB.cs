using Data.DataMetadata;
using Data.Enums;
using Data.Modifiers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelDB.Entities
{
    public class TypeMetadataDB : TypeMetadataBase
    {
        public TypeMetadataDB()
        {
            Constructors = new List<MethodMetadataDB>();
            Fields = new List<ParameterMetadataDB>();
            GenericArguments = new List<TypeMetadataDB>();
            ImplementedInterfaces = new List<TypeMetadataDB>();
            Methods = new List<MethodMetadataDB>();
            NestedTypes = new List<TypeMetadataDB>();
            Properties = new List<PropertyMetadataDB>();
        }

        [Key]
        public override string TypeName { get; set; }
        public override string AssemblyName { get; set; }
        public override bool IsExternal { get; set; }
        public override bool IsGeneric { get; set; }
        public override TypeModifiers Modifiers { get; set; }
        public override TypeKind TypeKind { get; set; }
        public new TypeMetadataDB BaseType { get; set; }
        public new TypeMetadataDB DeclaringType { get; set; }
        public new List<TypeMetadataDB> GenericArguments { get; set; }
        public new List<TypeMetadataDB> ImplementedInterfaces { get; set; }
        public new List<TypeMetadataDB> NestedTypes { get; set; }
        public new List<PropertyMetadataDB> Properties { get; set; }
        public new List<ParameterMetadataDB> Fields { get; set; }
        public new List<MethodMetadataDB> Methods { get; set; }
        public new List<MethodMetadataDB> Constructors { get; set; }
    }
}
