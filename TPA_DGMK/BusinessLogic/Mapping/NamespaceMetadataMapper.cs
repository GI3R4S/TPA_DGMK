using BusinessLogic.Model;
using Data.DataMetadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BusinessLogic.Mapping
{
    public class NamespaceMetadataMapper
    {
        public NamespaceMetadataBase MapToSerialize(NamespaceMetadata metadata, Type namespaceMetadataType)
        {
            object namespaceMetadata = Activator.CreateInstance(namespaceMetadataType);
            PropertyInfo nameProperty = namespaceMetadataType.GetProperty("NamespaceName");
            PropertyInfo namespaceMetadatasProperty = namespaceMetadataType.GetProperty("Types", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            nameProperty?.SetValue(namespaceMetadata, metadata.NamespaceName);
            namespaceMetadatasProperty?.SetValue(namespaceMetadata, ConvertionUtilities.ConvertList(namespaceMetadatasProperty.PropertyType.GetGenericArguments()[0],
                metadata.Types.Select(t => new TypeMetadataMapper().MapToSerialize(t, namespaceMetadatasProperty.PropertyType.GetGenericArguments()[0])).ToList()));
            return (NamespaceMetadataBase)namespaceMetadata;
        }

        public NamespaceMetadata MapToDeserialize(NamespaceMetadataBase metadata)
        {
            NamespaceMetadata namespaceMetadata = new NamespaceMetadata
            {
                NamespaceName = metadata.NamespaceName
            };
            Type type = metadata.GetType();
            PropertyInfo typesProperty = type.GetProperty("Types", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            List<TypeMetadataBase> types = (List<TypeMetadataBase>)ConvertionUtilities.ConvertList(typeof(TypeMetadataBase), (IList)typesProperty?.GetValue(metadata));
            if (types != null)
                namespaceMetadata.Types = types.Select(n => TypeMetadataMapper.EmitTypeForDeserialization(n)).ToList();
            return namespaceMetadata;
        }
    }
}
