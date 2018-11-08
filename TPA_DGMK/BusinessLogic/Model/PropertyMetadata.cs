using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Model
{
    public class PropertyMetadata
    {
        public string Name { get; private set; }
        public TypeMetadata TypeMetadata { get; private set; }
        public IEnumerable<TypeMetadata> AttributesMetadata { get; private set; }

        private PropertyMetadata(string propertyName, TypeMetadata propertyType)
        {
            Name = propertyName;
            TypeMetadata = propertyType;
        }

        internal static IEnumerable<PropertyMetadata> EmitProperties(IEnumerable<PropertyInfo> props)
        {
            return (from prop in props
                    where prop.GetGetMethod().GetVisible() || prop.GetSetMethod().GetVisible()
                    select new PropertyMetadata(prop.Name, TypeMetadata.EmitReference(prop.PropertyType))).ToList();
        }
    }
}
