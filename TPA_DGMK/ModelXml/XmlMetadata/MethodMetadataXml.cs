using Data.DataMetadata;
using Data.Enums;
using Data.Modifiers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelXml.XmlMetadata
{
    [DataContract(IsReference = true)]
    public class MethodMetadataXml : MethodMetadataBase
    {
        [DataMember] public override string Name { get; set; }
        [DataMember] public new List<TypeMetadataXml> GenericArguments { get; set; }
        [DataMember] public override MethodModifiers Modifiers { get; set; }
        [DataMember] public new TypeMetadataXml ReturnType { get; set; }
        [DataMember] public override bool Extension { get; set; }
        [DataMember] public new List<ParameterMetadataXml> Parameters { get; set; }
    }
}
