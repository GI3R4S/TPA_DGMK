﻿using Data.DataMetadata;
using System.Runtime.Serialization;

namespace ModelXml.XmlMetadata
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(TypeMetadataXml))]
    public class PropertyMetadataXml : PropertyMetadataBase
    {
        [DataMember] public override string Name { get; set; }
        [DataMember] public new TypeMetadataXml TypeMetadata { get; set; }
    }
}
