using Data.DataMetadata;
using Data.Modifiers;
using System.Collections.Generic;

namespace ModelDB.Entities
{
    public class MethodMetadataDB : MethodMetadataBase
    {
        public int Id { get; set; }
        public override string Name { get; set; }
        public override bool Extension { get; set; }
        public override MethodModifiers Modifiers { get; set; }
        public new List<TypeMetadataDB> GenericArguments { get; set; }
        public new TypeMetadataDB ReturnType { get; set; }
        public new List<ParameterMetadataDB> Parameters { get; set; }
    }
}
