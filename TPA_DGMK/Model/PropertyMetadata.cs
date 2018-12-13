using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Model
{
    [DataContract(IsReference = true)]
    public class PropertyMetadata
    {
        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public TypeMetadata TypeMetadata { get; private set; }
        [DataMember]
        public ICollection<TypeMetadata> AttributesMetadata { get; private set; }

        private PropertyMetadata(string propertyName, TypeMetadata propertyType)
        {
            Id = ++counter;
            Name = propertyName;
            TypeMetadata = propertyType;
        }

        private PropertyMetadata() { }
        private static int counter = 0;

        internal static ICollection<PropertyMetadata> EmitProperties(ICollection<PropertyInfo> props)
        {
            return (from prop in props
                    where prop.GetGetMethod().GetVisible() || prop.GetSetMethod().GetVisible()
                    select new PropertyMetadata(prop.Name, TypeMetadata.EmitReference(prop.PropertyType))).ToList();
        }
    }
}
