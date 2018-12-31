using BusinessLogic.Model;
using Data.DataMetadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BusinessLogic.Mapping
{
    public class MethodMetadataMapper
    {
        public MethodMetadataBase MapToSerialize(MethodMetadata model, Type methodMetadataType)
        {
            object methodMetadata = Activator.CreateInstance(methodMetadataType);
            PropertyInfo nameProperty = methodMetadataType.GetProperty("Name");
            PropertyInfo extensionProperty = methodMetadataType.GetProperty("Extension");
            PropertyInfo genericArgumentsProperty = methodMetadataType.GetProperty("GenericArguments", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            PropertyInfo modifiersProperty = methodMetadataType.GetProperty("Modifiers");
            PropertyInfo parametersProperty = methodMetadataType.GetProperty("Parameters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            PropertyInfo returnTypeProperty = methodMetadataType.GetProperty("ReturnType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            nameProperty?.SetValue(methodMetadata, model.Name);
            extensionProperty?.SetValue(methodMetadata, model.Extension);
            if (model.GenericArguments != null)
                genericArgumentsProperty?.SetValue(methodMetadata, ConvertionUtilities.ConvertList(genericArgumentsProperty.PropertyType.GetGenericArguments()[0],
                    model.GenericArguments.Select(t => TypeMetadataMapper.EmitTypeToSerialization(t, genericArgumentsProperty.PropertyType.GetGenericArguments()[0])).ToList()));
            modifiersProperty?.SetValue(methodMetadata, model.Modifiers);
            if (model.Parameters != null)
                parametersProperty?.SetValue(methodMetadata, ConvertionUtilities.ConvertList(parametersProperty.PropertyType.GetGenericArguments()[0],
                    model.Parameters.Select(p => new ParameterMetadataMapper().MapToSerialize(p, parametersProperty.PropertyType.GetGenericArguments()[0])).ToList()));
            if (model.ReturnType != null)
                returnTypeProperty?.SetValue(methodMetadata, returnTypeProperty.PropertyType.Cast(TypeMetadataMapper.EmitTypeToSerialization(model.ReturnType, returnTypeProperty.PropertyType)));
            return (MethodMetadataBase)methodMetadata;
        }

        public MethodMetadata MapToDeserialize(MethodMetadataBase metadata)
        {
            MethodMetadata methodMetadata = new MethodMetadata
            {
                Name = metadata.Name,
                Extension = metadata.Extension
            };
            Type type = metadata.GetType();
            PropertyInfo genericArgumentsProperty = type.GetProperty("GenericArguments", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (genericArgumentsProperty?.GetValue(metadata) != null)
            {
                List<TypeMetadataBase> genericArguments = (List<TypeMetadataBase>)ConvertionUtilities.ConvertList(typeof(TypeMetadataBase), (IList)genericArgumentsProperty?.GetValue(metadata));
                methodMetadata.GenericArguments = genericArguments.Select(g => TypeMetadataMapper.EmitTypeForDeserialization(g)).ToList();
            }
            methodMetadata.Modifiers = metadata.Modifiers;
            PropertyInfo parametersProperty = type.GetProperty("Parameters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (parametersProperty?.GetValue(metadata) != null)
            {
                List<ParameterMetadataBase> parameters = (List<ParameterMetadataBase>)ConvertionUtilities.ConvertList(typeof(ParameterMetadataBase), (IList)parametersProperty?.GetValue(metadata));
                methodMetadata.Parameters = parameters.Select(p => new ParameterMetadataMapper().MapToDeserialize(p)).ToList();
            }
            PropertyInfo returnTypeProperty = type.GetProperty("ReturnType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            TypeMetadataBase returnType = (TypeMetadataBase)returnTypeProperty?.GetValue(metadata);
            if (returnType != null)
                methodMetadata.ReturnType = TypeMetadataMapper.EmitTypeForDeserialization(returnType);
            return methodMetadata;
        }
    }
}
