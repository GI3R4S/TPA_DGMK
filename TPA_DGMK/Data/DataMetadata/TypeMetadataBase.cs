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
        [DataMember] public virtual string NamespaceName { get; set; }
        [DataMember] public virtual string FullTypeName { get; set; }
        public virtual TypeMetadataBase BaseType { get; set; }
        public virtual List<TypeMetadataBase> GenericArguments { get; set; }
        [DataMember] public virtual Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers { get; set; }
        [DataMember] public virtual TypeKind TypeKind { get; set; }
        public virtual List<TypeMetadataBase> ImplementedInterfaces { get; set; }
        public virtual List<TypeMetadataBase> NestedTypes { get; set; }
        public virtual List<PropertyMetadataBase> Properties { get; set; }
        public virtual TypeMetadataBase DeclaringType { get; set; }
        public virtual List<FieldMetadataBase> Fields { get; set; }
        public virtual List<MethodMetadataBase> Methods { get; set; }
        public virtual List<MethodMetadataBase> Constructors { get; set; }
        [DataMember] public virtual AccessLevel AccessLevel { get; set; }
        [DataMember] public virtual bool IsAbstract { get; set; }
        [DataMember] public virtual bool IsSealed { get; set; }
    }
}
