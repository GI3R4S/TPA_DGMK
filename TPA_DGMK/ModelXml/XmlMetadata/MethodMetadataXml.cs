using Data.DataMetadata;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelXml.XmlMetadata
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(TypeMetadataXml))]
    public class MethodMetadataXml : MethodMetadataBase
    {
        [DataMember] public override string Name { get; set; }
        [DataMember] public new List<TypeMetadataXml> GenericArguments { get; set; }
        [DataMember] public override Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; set; }
        [DataMember] public new TypeMetadataXml ReturnType { get; set; }
        [DataMember] public override bool Extension { get; set; }
        [DataMember] public new List<ParameterMetadataXml> Parameters { get; set; }
    }
}
