using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Model
{
    [DataContract(IsReference = true)]
    public class ParameterMetadata
    {
        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public TypeMetadata TypeMetadata { get; private set; }
        [DataMember]
        public ICollection<TypeMetadata> Attributes { get; private set; }

        public ParameterMetadata(string name, TypeMetadata typeMetadata)
        {
            Id = ++counter;
            this.Name = name;
            this.TypeMetadata = typeMetadata;
        }

        private ParameterMetadata() { }
        private static int counter = 0;
    }
}
