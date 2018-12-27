using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BusinessLogic.Model
{
    public class PropertyMetadata
    {
        public string Name { get; set; }
        public TypeMetadata TypeMetadata { get; set; }

        private PropertyMetadata(string propertyName, TypeMetadata propertyType)
        {
            Name = propertyName;
            TypeMetadata = propertyType;
        }

        public PropertyMetadata() { }

        internal static List<PropertyMetadata> EmitProperties(Type type)
        {
            List<PropertyInfo> props = type.GetProperties(BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Public |
                   BindingFlags.Static | BindingFlags.Instance).ToList();
            return (from prop in props
                    where prop.GetGetMethod().GetVisible() || prop.GetSetMethod().GetVisible()
                    select new PropertyMetadata(prop.Name, TypeMetadata.EmitReference(prop.PropertyType))).ToList();
        }
    }
}
