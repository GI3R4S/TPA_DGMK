using BusinessLogic.Model;
using Data.DataMetadata;
using System;
using System.Reflection;

namespace BusinessLogic.Mapping
{
    public class PropertyMetadataMapper
    {
        public PropertyMetadata MapUp(PropertyMetadataBase metadata)
        {
            PropertyMetadata propertyMetadata = new PropertyMetadata
            {
                Name = metadata.Name
            };
            Type type = metadata.GetType();
            PropertyInfo typeProperty = type.GetProperty("TypeMetadata",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            TypeMetadataBase typeModel = (TypeMetadataBase)typeProperty?.GetValue(metadata);

            if (typeModel != null)
                propertyMetadata.TypeMetadata = TypeMetadataMapper.EmitType(typeModel);

            return propertyMetadata;
        }

        public PropertyMetadataBase MapDown(PropertyMetadata metadata,Type propertyMetadataType)
        {
            object propertyMetadata = Activator.CreateInstance(propertyMetadataType);
            PropertyInfo nameProperty = propertyMetadataType.GetProperty("Name");
            PropertyInfo typeProperty = propertyMetadataType.GetProperty("TypeMetadata", 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            nameProperty?.SetValue(propertyMetadata, metadata.Name);

            if (metadata.TypeMetadata != null)
                typeProperty?.SetValue(propertyMetadata, 
                    typeProperty.PropertyType.Cast(TypeMetadataMapper.EmitBaseType(metadata.TypeMetadata, typeProperty.PropertyType)));

            return (PropertyMetadataBase)propertyMetadata;
        }
    }
}
