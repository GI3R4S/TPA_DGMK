using Data.Enums;
using System;
using System.Collections.Generic;

namespace Data.DataMetadata
{
    public abstract class TypeMetadataBase
    {
        public virtual string TypeName { get; set; }
        public virtual string NamespaceName { get; set; }
        public virtual string FullTypeName { get; set; }
        public virtual TypeMetadataBase BaseType { get; set; }
        public virtual List<TypeMetadataBase> GenericArguments { get; set; }
        public virtual Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers { get; set; }
        public virtual TypeKind TypeKind { get; set; }
        public virtual List<TypeMetadataBase> ImplementedInterfaces { get; set; }
        public virtual List<TypeMetadataBase> NestedTypes { get; set; }
        public virtual List<PropertyMetadataBase> Properties { get; set; }
        public virtual TypeMetadataBase DeclaringType { get; set; }
        public virtual List<FieldMetadataBase> Fields { get; set; }
        public virtual List<MethodMetadataBase> Methods { get; set; }
        public virtual List<MethodMetadataBase> Constructors { get; set; }
        public virtual AccessLevel AccessLevel { get; set; }
        public virtual bool IsAbstract { get; set; }
        public virtual bool IsSealed { get; set; }
    }
}
