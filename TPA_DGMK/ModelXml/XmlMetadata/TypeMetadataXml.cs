using Data.DataMetadata;
using Data.Enums;
using Data.Modifiers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelXml.XmlMetadata
{
    [DataContract(IsReference = true)]
    public class TypeMetadataXml : TypeMetadataBase
    {
        [DataMember] public override string TypeName { get; set; }
        [DataMember] public override string AssemblyName { get; set; }
        [DataMember] public override bool IsGeneric { get; set; }
        [DataMember] public override bool IsExternal { get; set; }
        [DataMember] public new TypeMetadataXml BaseType { get; set; }
        [DataMember] public new List<TypeMetadataXml> GenericArguments { get; set; }
        [DataMember] public override TypeModifiers Modifiers { get; set; }
        [DataMember] public override TypeKind TypeKind { get; set; }
        [DataMember] public new List<TypeMetadataXml> ImplementedInterfaces { get; set; }
        [DataMember] public new List<TypeMetadataXml> NestedTypes { get; set; }
        [DataMember] public new List<PropertyMetadataXml> Properties { get; set; }
        [DataMember] public new List<ParameterMetadataXml> Fields { get; set; }
        [DataMember] public new TypeMetadataXml DeclaringType { get; set; }
        [DataMember] public new List<MethodMetadataXml> Methods { get; set; }
        [DataMember] public new List<MethodMetadataXml> Constructors { get; set; }
    }
}
