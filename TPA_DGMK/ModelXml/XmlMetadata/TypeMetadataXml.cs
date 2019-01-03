using Data.DataMetadata;
using Data.Enums;
using Data.Modifiers;
using System.Collections.Generic;

namespace ModelXml.XmlMetadata
{
    public class TypeMetadataXml : TypeMetadataBase
    {
        public override string TypeName { get; set; }
        public override string AssemblyName { get; set; }
        public override bool IsGeneric { get; set; }
        public override bool IsExternal { get; set; }
        public new TypeMetadataXml BaseType { get; set; }
        public new List<TypeMetadataXml> GenericArguments { get; set; }
        public override TypeModifiers Modifiers { get; set; }
        public override TypeKind TypeKind { get; set; }
        public new List<TypeMetadataXml> ImplementedInterfaces { get; set; }
        public new List<TypeMetadataXml> NestedTypes { get; set; }
        public new List<PropertyMetadataXml> Properties { get; set; }
        public new List<ParameterMetadataXml> Fields { get; set; }
        public new TypeMetadataXml DeclaringType { get; set; }
        public new List<MethodMetadataXml> Methods { get; set; }
        public new List<MethodMetadataXml> Constructors { get; set; }
    }
}
