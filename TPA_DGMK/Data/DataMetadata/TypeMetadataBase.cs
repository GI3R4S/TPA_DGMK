using Data.Enums;
using Data.Modifiers;
using System.Collections.Generic;

namespace Data.DataMetadata
{
    public abstract class TypeMetadataBase
    {
        public virtual string TypeName { get; set; }
        public virtual string AssemblyName { get; set; }
        public virtual bool IsGeneric { get; set; }
        public virtual bool IsExternal { get; set; }
        public virtual TypeMetadataBase BaseType { get; set; }
        public virtual List<TypeMetadataBase> GenericArguments { get; set; }
        public virtual TypeModifiers Modifiers { get; set; }
        public virtual TypeKind TypeKind { get; set; }
        public virtual List<TypeMetadataBase> ImplementedInterfaces { get; set; }
        public virtual List<TypeMetadataBase> NestedTypes { get; set; }
        public virtual List<PropertyMetadataBase> Properties { get; set; }
        public virtual TypeMetadataBase DeclaringType { get; set; }
        public virtual List<ParameterMetadataBase> Fields { get; set; }
        public virtual List<MethodMetadataBase> Methods { get; set; }
        public virtual List<MethodMetadataBase> Constructors { get; set; }
    }
}
