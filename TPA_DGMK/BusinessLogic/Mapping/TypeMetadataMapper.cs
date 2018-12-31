using BusinessLogic.Model;
using Data.DataMetadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BusinessLogic.Mapping
{
    public class TypeMetadataMapper
    {
        public static Dictionary<string, TypeMetadataBase> TypesForSerialization = new Dictionary<string, TypeMetadataBase>();
        public static Dictionary<string, TypeMetadata> TypesForDeserialization = new Dictionary<string, TypeMetadata>();

        public TypeMetadataBase MapToSerialize(TypeMetadata typeMetadata, Type type)
        {
            object obj = Activator.CreateInstance(type);
            if (typeMetadata == null)
                return null;
            if (!TypesForSerialization.ContainsKey(typeMetadata.TypeName))
            {
                TypesForSerialization.Add(typeMetadata.TypeName, (TypeMetadataBase)obj);
                FillTypeForSerialization(typeMetadata, (TypeMetadataBase)obj);
            }
            return TypesForSerialization[typeMetadata.TypeName];
        }

        public TypeMetadata MapToDeserialize(TypeMetadataBase metadata)
        {
            TypeMetadata typeMetadata = new TypeMetadata();
            if (metadata == null)
                return null;
            if (!TypesForDeserialization.ContainsKey(metadata.TypeName))
            {
                TypesForDeserialization.Add(metadata.TypeName, typeMetadata);
                FillTypeForDeserialization(metadata, typeMetadata);
            }
            return TypesForDeserialization[metadata.TypeName];
        }

        public static TypeMetadataBase EmitTypeToSerialization(TypeMetadata metadata, Type type)
        {
            return new TypeMetadataMapper().MapToSerialize(metadata, type);
        }

        public static TypeMetadata EmitTypeForDeserialization(TypeMetadataBase metadata)
        {
            return new TypeMetadataMapper().MapToDeserialize(metadata);
        }

        private void FillTypeForSerialization(TypeMetadata metadata, TypeMetadataBase typeMetadata)
        {
            Type type = typeMetadata.GetType();

            type.GetProperty("TypeName")?.SetValue(typeMetadata, metadata.TypeName);
            type.GetProperty("AssemblyName")?.SetValue(typeMetadata, metadata.AssemblyName);
            type.GetProperty("TypeKind")?.SetValue(typeMetadata, metadata.TypeKind);
            type.GetProperty("IsExternal")?.SetValue(typeMetadata, metadata.IsExternal);
            type.GetProperty("IsGeneric")?.SetValue(typeMetadata, metadata.IsGeneric);
            type.GetProperty("Modifiers")?.SetValue(typeMetadata, metadata.Modifiers);

            if (metadata.BaseType != null)
            {
                type.GetProperty("BaseType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    ?.SetValue(typeMetadata, type.Cast(EmitTypeToSerialization(metadata.BaseType, type)));
            }

            if (metadata.DeclaringType != null)
            {
                type.GetProperty("DeclaringType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    ?.SetValue(typeMetadata, type.Cast(EmitTypeToSerialization(metadata.DeclaringType, type)));
            }

            if (metadata.NestedTypes != null)
            {
                PropertyInfo nestedTypesProperty = type.GetProperty("NestedTypes", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                nestedTypesProperty?.SetValue(typeMetadata, ConvertionUtilities.ConvertList(type,
                    metadata.NestedTypes?.Select(c => EmitTypeToSerialization(c, type)).ToList()));
            }

            if (metadata.GenericArguments != null)
            {
                PropertyInfo genericArgumentsProperty = type.GetProperty("GenericArguments", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                genericArgumentsProperty?.SetValue(typeMetadata, ConvertionUtilities.ConvertList(type,
                    metadata.GenericArguments?.Select(c => EmitTypeToSerialization(c, type)).ToList()));
            }

            if (metadata.ImplementedInterfaces != null)
            {
                PropertyInfo implementedInterfacesProperty = type.GetProperty("ImplementedInterfaces", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                implementedInterfacesProperty?.SetValue(typeMetadata, ConvertionUtilities.ConvertList(type,
                    metadata.ImplementedInterfaces?.Select(c => EmitTypeToSerialization(c, type)).ToList()));
            }

            if (metadata.Fields != null)
            {
                PropertyInfo fieldsProperty = type.GetProperty("Fields", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                fieldsProperty?.SetValue(typeMetadata, ConvertionUtilities.ConvertList(fieldsProperty.PropertyType.GetGenericArguments()[0],
                    metadata.Fields?.Select(c => new ParameterMetadataMapper().MapToSerialize(c, fieldsProperty?.PropertyType.GetGenericArguments()[0])).ToList()));
            }

            if (metadata.Methods != null)
            {
                PropertyInfo methodsProperty = type.GetProperty("Methods", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                methodsProperty?.SetValue(typeMetadata, ConvertionUtilities.ConvertList(methodsProperty.PropertyType.GetGenericArguments()[0],
                    metadata.Methods?.Select(m => new MethodMetadataMapper().MapToSerialize(m, methodsProperty?.PropertyType.GetGenericArguments()[0])).ToList()));
            }

            if (metadata.Constructors != null)
            {
                PropertyInfo constructorsProperty = type.GetProperty("Constructors", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                constructorsProperty?.SetValue(typeMetadata, ConvertionUtilities.ConvertList(constructorsProperty.PropertyType.GetGenericArguments()[0],
                    metadata.Constructors?.Select(c => new MethodMetadataMapper().MapToSerialize(c, constructorsProperty?.PropertyType.GetGenericArguments()[0])).ToList()));
            }

            if (metadata.Properties != null)
            {
                PropertyInfo propertiesProperty = type.GetProperty("Properties", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                propertiesProperty?.SetValue(typeMetadata, ConvertionUtilities.ConvertList(propertiesProperty.PropertyType.GetGenericArguments()[0],
                    metadata.Properties?.Select(c => new PropertyMetadataMapper().MapToSerialize(c, propertiesProperty?.PropertyType.GetGenericArguments()[0])).ToList()));
            }
        }

        private void FillTypeForDeserialization(TypeMetadataBase metadata, TypeMetadata typeMetadata)
        {
            typeMetadata.TypeName = metadata.TypeName;
            typeMetadata.AssemblyName = metadata.AssemblyName;
            typeMetadata.TypeKind = metadata.TypeKind;
            typeMetadata.IsExternal = metadata.IsExternal;
            typeMetadata.IsGeneric = metadata.IsGeneric;
            typeMetadata.Modifiers = metadata.Modifiers;

            Type type = metadata.GetType();
            PropertyInfo baseTypeProperty = type.GetProperty("BaseType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            TypeMetadataBase baseType = (TypeMetadataBase)baseTypeProperty?.GetValue(metadata);
            typeMetadata.BaseType = EmitTypeForDeserialization(baseType);

            PropertyInfo declaringTypeProperty = type.GetProperty("DeclaringType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            TypeMetadataBase declaringType = (TypeMetadataBase)declaringTypeProperty?.GetValue(metadata);
            typeMetadata.DeclaringType = EmitTypeForDeserialization(declaringType);

            PropertyInfo nestedTypesProperty = type.GetProperty("NestedTypes", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (nestedTypesProperty?.GetValue(metadata) != null)
            {
                List<TypeMetadataBase> nestedTypes = (List<TypeMetadataBase>)ConvertionUtilities.ConvertList(typeof(TypeMetadataBase), (IList)nestedTypesProperty?.GetValue(metadata));
                typeMetadata.NestedTypes = nestedTypes?.Select(n => EmitTypeForDeserialization(n)).ToList();
            }

            PropertyInfo genericArgumentsProperty = type.GetProperty("GenericArguments", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (genericArgumentsProperty?.GetValue(metadata) != null)
            {
                List<TypeMetadataBase> genericArguments = (List<TypeMetadataBase>)ConvertionUtilities.ConvertList(typeof(TypeMetadataBase), (IList)genericArgumentsProperty?.GetValue(metadata));
                typeMetadata.GenericArguments = genericArguments?.Select(g => EmitTypeForDeserialization(g)).ToList();
            }

            PropertyInfo implementedInterfacesProperty = type.GetProperty("ImplementedInterfaces", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (implementedInterfacesProperty?.GetValue(metadata) != null)
            {
                List<TypeMetadataBase> implementedInterfaces = (List<TypeMetadataBase>)ConvertionUtilities.ConvertList(typeof(TypeMetadataBase), (IList)implementedInterfacesProperty?.GetValue(metadata));
                typeMetadata.ImplementedInterfaces = implementedInterfaces?.Select(i => EmitTypeForDeserialization(i)).ToList();
            }

            PropertyInfo fieldsProperty = type.GetProperty("Fields", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (fieldsProperty?.GetValue(metadata) != null)
            {
                List<ParameterMetadataBase> fields = (List<ParameterMetadataBase>)ConvertionUtilities.ConvertList(typeof(ParameterMetadataBase), (IList)fieldsProperty?.GetValue(metadata));
                typeMetadata.Fields = fields?.Select(c => new ParameterMetadataMapper().MapToDeserialize(c)).ToList();
            }

            PropertyInfo methodsProperty = type.GetProperty("Methods", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (methodsProperty?.GetValue(metadata) != null)
            {
                List<MethodMetadataBase> methods = (List<MethodMetadataBase>)ConvertionUtilities.ConvertList(typeof(MethodMetadataBase), (IList)methodsProperty?.GetValue(metadata));
                typeMetadata.Methods = methods?.Select(c => new MethodMetadataMapper().MapToDeserialize(c)).ToList();
            }

            PropertyInfo constructorsProperty = type.GetProperty("Constructors", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (constructorsProperty?.GetValue(metadata) != null)
            {
                List<MethodMetadataBase> constructors = (List<MethodMetadataBase>)ConvertionUtilities.ConvertList(typeof(MethodMetadataBase), (IList)constructorsProperty?.GetValue(metadata));
                typeMetadata.Constructors = constructors?.Select(c => new MethodMetadataMapper().MapToDeserialize(c)).ToList();
            }

            PropertyInfo propertiesProperty = type.GetProperty("Properties", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (propertiesProperty?.GetValue(metadata) != null)
            {
                List<PropertyMetadataBase> properties = (List<PropertyMetadataBase>)ConvertionUtilities.ConvertList(typeof(PropertyMetadataBase), (IList)propertiesProperty?.GetValue(metadata));
                typeMetadata.Properties = properties?.Select(g => new PropertyMetadataMapper().MapToDeserialize(g)).ToList();
            }
        }
    }
}