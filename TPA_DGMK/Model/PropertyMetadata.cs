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
        public string Name { get; private set; }
        [DataMember]
        public TypeMetadata TypeMetadata { get; private set; }
        [DataMember]
        public IEnumerable<TypeMetadata> AttributesMetadata { get; private set; }

        private PropertyMetadata(string propertyName, TypeMetadata propertyType)
        {
            Name = propertyName;
            TypeMetadata = propertyType;
        }

        private PropertyMetadata() { }

        internal static IEnumerable<PropertyMetadata> EmitProperties(IEnumerable<PropertyInfo> props)
        {
            return (from prop in props
                    where prop.GetGetMethod().GetVisible() || prop.GetSetMethod().GetVisible()
                    select new PropertyMetadata(prop.Name, TypeMetadata.EmitReference(prop.PropertyType))).ToList();
        }
    }
}
