using BusinessLogic.Model;
using Data.DataMetadata;
using System;
using System.Reflection;

namespace BusinessLogic.Mapping
{
    public class ParameterMetadataMapper
    {
        public ParameterMetadataBase MapToSerialize(ParameterMetadata metadata, Type parameterMetadataType)
        {
            object parameterMetadata = Activator.CreateInstance(parameterMetadataType);
            PropertyInfo nameProperty = parameterMetadataType.GetProperty("Name");
            PropertyInfo typeProperty = parameterMetadataType.GetProperty("TypeMetadata", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            nameProperty?.SetValue(parameterMetadata, metadata.Name);
            if (metadata.TypeMetadata != null)
                typeProperty?.SetValue(parameterMetadata, typeProperty.PropertyType.Cast(TypeMetadataMapper.EmitTypeToSerialization(metadata.TypeMetadata, typeProperty.PropertyType)));
            return (ParameterMetadataBase)parameterMetadata;
        }

        public ParameterMetadata MapToDeserialize(ParameterMetadataBase metadata)
        {
            ParameterMetadata parameterMetadata = new ParameterMetadata
            {
                Name = metadata.Name
            };
            Type type = metadata.GetType();
            PropertyInfo typeProperty = type.GetProperty("TypeMetadata", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            TypeMetadataBase typeModel = (TypeMetadataBase)typeProperty?.GetValue(metadata);
            if (typeModel != null)
                parameterMetadata.TypeMetadata = TypeMetadataMapper.EmitTypeForDeserialization(typeModel);
            return parameterMetadata;
        }
    }
}
