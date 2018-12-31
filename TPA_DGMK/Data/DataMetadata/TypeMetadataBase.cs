using Data.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Data.DataMetadata
{
    [DataContract(IsReference = true)]
    public abstract class TypeMetadataBase
    {
        [DataMember] public virtual string TypeName { get; set; }
        [DataMember] public virtual string AssemblyName { get; set; }
        [DataMember] public virtual bool IsGeneric { get; set; }
        [DataMember] public virtual bool IsExternal { get; set; }
        public virtual TypeMetadataBase BaseType { get; set; }
        public virtual List<TypeMetadataBase> GenericArguments { get; set; }
        [DataMember] public virtual Tuple<AccessLevel, SealedEnum, AbstractEnum, StaticEnum> Modifiers { get; set; }
        [DataMember] public virtual TypeKind TypeKind { get; set; }
        public virtual List<TypeMetadataBase> ImplementedInterfaces { get; set; }
        public virtual List<TypeMetadataBase> NestedTypes { get; set; }
        public virtual List<PropertyMetadataBase> Properties { get; set; }
        public virtual TypeMetadataBase DeclaringType { get; set; }
        public virtual List<ParameterMetadataBase> Fields { get; set; }
        public virtual List<MethodMetadataBase> Methods { get; set; }
        public virtual List<MethodMetadataBase> Constructors { get; set; }
    }
}
