using Data.Enums;
using System;
using System.Collections.Generic;

namespace Data.DataMetadata
{
    public abstract class MethodMetadataBase
    {
        public virtual string Name { get; set; }
        public virtual List<TypeMetadataBase> GenericArguments { get; set; }
        public virtual Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; set; }
        public virtual TypeMetadataBase ReturnType { get; set; }
        public virtual bool Extension { get; set; }
        public virtual List<ParameterMetadataBase> Parameters { get; set; }
    }
}
