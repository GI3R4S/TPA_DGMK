using Data.DataMetadata;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelXml.XmlMetadata
{
    [DataContract(IsReference = true)]
    public class TypeMetadataXml : TypeMetadataBase
    {
        [DataMember] public override string TypeName { get; set; }
        [DataMember] public override string NamespaceName { get; set; }
        [DataMember] public override string FullTypeName { get; set; }
        [DataMember] public new TypeMetadataXml BaseType { get; set; }
        [DataMember] public new List<TypeMetadataXml> GenericArguments { get; set; }
        [DataMember] public override Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers { get; set; }
        [DataMember] public override TypeKind TypeKind { get; set; }
        [DataMember] public new List<TypeMetadataXml> ImplementedInterfaces { get; set; }
        [DataMember] public new List<TypeMetadataXml> NestedTypes { get; set; }
        [DataMember] public new List<PropertyMetadataXml> Properties { get; set; }
        [DataMember] public new List<FieldMetadataXml> Fields { get; set; }
        [DataMember] public new TypeMetadataBase DeclaringType { get; set; }
        [DataMember] public new List<MethodMetadataXml> Methods { get; set; }
        [DataMember] public new List<MethodMetadataXml> Constructors { get; set; }
        [DataMember] public override bool IsAbstract { get; set; }
        [DataMember] public override bool IsSealed { get; set; }
    }
}
