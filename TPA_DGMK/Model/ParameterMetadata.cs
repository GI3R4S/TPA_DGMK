using System.Collections.Generic;

namespace Model
{
    public class ParameterMetadata
    {
        public string Name { get; private set; }
        public TypeMetadata TypeMetadata { get; private set; }
        public IEnumerable<TypeMetadata> Attributes { get; private set; }

        public ParameterMetadata(string name, TypeMetadata typeMetadata)
        {
            this.Name = name;
            this.TypeMetadata = typeMetadata;
        }
    }
}
