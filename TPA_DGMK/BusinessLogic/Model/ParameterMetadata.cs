using System.Collections.Generic;

namespace BusinessLogic.Model
{
    public class ParameterMetadata
    {
        public string Name { get; set; }
        public TypeMetadata TypeMetadata { get; set; }

        public ParameterMetadata(string name, TypeMetadata typeMetadata)
        {
            this.Name = name;
            this.TypeMetadata = typeMetadata;
        }

        public ParameterMetadata() { }
    }
}
