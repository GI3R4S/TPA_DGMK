using Data.DataMetadata;
using Data.Modifiers;
using System.Collections.Generic;

namespace ModelXml.XmlMetadata
{
    public class MethodMetadataXml : MethodMetadataBase
    {
        public override string Name { get; set; }
        public new List<TypeMetadataXml> GenericArguments { get; set; }
        public override MethodModifiers Modifiers { get; set; }
        public new TypeMetadataXml ReturnType { get; set; }
        public override bool Extension { get; set; }
        public new List<ParameterMetadataXml> Parameters { get; set; }
    }
}
