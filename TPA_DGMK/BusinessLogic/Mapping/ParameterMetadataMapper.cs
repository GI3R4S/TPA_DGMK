using BusinessLogic.Model;
using Data.DataMetadata;
using System;
using System.Reflection;

namespace BusinessLogic.Mapping
{
    public class ParameterMetadataMapper
    {
        public ParameterMetadata MapUp(ParameterMetadataBase metadata)
        {
            ParameterMetadata parameterMetadata = new ParameterMetadata
            {
                Name = metadata.Name
            };
            Type type = metadata.GetType();
            PropertyInfo typeProperty = type.GetProperty("TypeMetadata",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            TypeMetadataBase typeModel = (TypeMetadataBase)typeProperty?.GetValue(metadata);
            if (typeModel != null)
                parameterMetadata.TypeMetadata = TypeMetadataMapper.EmitType(typeModel);
            return parameterMetadata;
        }

        public ParameterMetadataBase MapDown(ParameterMetadata metadata, Type parameterMetadataType)
        {
            object parameterMetadata = Activator.CreateInstance(parameterMetadataType);
            PropertyInfo nameProperty = parameterMetadataType.GetProperty("Name");
            PropertyInfo typeProperty = parameterMetadataType.GetProperty("TypeMetadata",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            nameProperty?.SetValue(parameterMetadata, metadata.Name);
            if (metadata.TypeMetadata != null)
                typeProperty?.SetValue(parameterMetadata,
                    typeProperty.PropertyType.Cast(TypeMetadataMapper.EmitBaseType(metadata.TypeMetadata, typeProperty.PropertyType)));

            return (ParameterMetadataBase)parameterMetadata;
        }
    }
}
