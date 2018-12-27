using BusinessLogic.Model;
using Data.DataMetadata;
using System;
using System.Reflection;

namespace BusinessLogic.Mapping
{
    public class FieldMetadataMapper
    {
        public FieldMetadata MapUp(FieldMetadataBase metadata)
        {
            FieldMetadata fieldMetadata = new FieldMetadata
            {
                Name = metadata.Name
            };
            Type type = metadata.GetType();
            PropertyInfo typeProperty = type.GetProperty("TypeMetadata",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            TypeMetadataBase typeModel = (TypeMetadataBase)typeProperty?.GetValue(metadata);
            if (typeModel != null)
                fieldMetadata.TypeMetadata = TypeMetadataMapper.EmitType(typeModel);
            return fieldMetadata;
        }

        public FieldMetadataBase MapDown(FieldMetadata metadata, Type fieldMetadataType)
        {
            object fieldMetadata = Activator.CreateInstance(fieldMetadataType);
            PropertyInfo nameProperty = fieldMetadataType.GetProperty("Name");
            PropertyInfo typeProperty = fieldMetadataType.GetProperty("TypeMetadata",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            nameProperty?.SetValue(fieldMetadata, metadata.Name);
            if (metadata.TypeMetadata != null)
                typeProperty?.SetValue(fieldMetadata,
                    typeProperty.PropertyType.Cast(TypeMetadataMapper.EmitBaseType(metadata.TypeMetadata, typeProperty.PropertyType)));

            return (FieldMetadataBase)fieldMetadata;
        }
    }
}
