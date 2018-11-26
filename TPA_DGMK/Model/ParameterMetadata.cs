using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Model
{
    [DataContract(IsReference = true)]
    public class ParameterMetadata
    {
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public TypeMetadata TypeMetadata { get; private set; }
        [DataMember]
        public IEnumerable<TypeMetadata> Attributes { get; private set; }

        public ParameterMetadata(string name, TypeMetadata typeMetadata)
        {
            this.Name = name;
            this.TypeMetadata = typeMetadata;
        }

        private ParameterMetadata() { }
    }
}
