using Data.Enums;
using Data.Modifiers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Data.DataMetadata
{
    [DataContract(IsReference = true)]
    public abstract class MethodMetadataBase
    {
        [DataMember] public virtual string Name { get; set; }
        public virtual List<TypeMetadataBase> GenericArguments { get; set; }
        [DataMember] public virtual MethodModifiers Modifiers { get; set; }
        public virtual TypeMetadataBase ReturnType { get; set; }
        [DataMember] public virtual bool Extension { get; set; }
        public virtual List<ParameterMetadataBase> Parameters { get; set; }
    }
}
