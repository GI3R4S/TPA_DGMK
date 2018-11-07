using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Model
{
    public class PropertyMetadata
    {
        private string m_Name;
        private TypeMetadata m_TypeMetadata;
        private IEnumerable<TypeMetadata> m_AttributesMetadata;

        public string Name { get => m_Name; set => m_Name = value; }
        public TypeMetadata TypeMetadata { get => m_TypeMetadata; set => m_TypeMetadata = value; }
        public IEnumerable<TypeMetadata> AttributesMetadata { get => m_AttributesMetadata; set => m_AttributesMetadata = value; }

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
