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
        public NamespaceMetadata MapUp(NamespaceMetadataBase metadata)
        {
            NamespaceMetadata namespaceMetadata = new NamespaceMetadata
            {
                NamespaceName = metadata.NamespaceName
            };
            Type type = metadata.GetType();
            PropertyInfo typesProperty = type.GetProperty("Types",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            List<TypeMetadataBase> types = (List<TypeMetadataBase>)HelperClass.ConvertList(typeof(TypeMetadataBase), (IList)typesProperty?.GetValue(metadata));
            if (types != null)
                namespaceMetadata.Types = types.Select(n => TypeMetadataMapper.EmitType(n)).ToList();
            return namespaceMetadata;
        }

        public NamespaceMetadataBase MapDown(NamespaceMetadata metadata, Type namespaceMetadataType)
        {
            object namespaceMetadata = Activator.CreateInstance(namespaceMetadataType);
            PropertyInfo nameProperty = namespaceMetadataType.GetProperty("NamespaceName");
            PropertyInfo namespaceMetadatasProperty = namespaceMetadataType.GetProperty("Types",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            nameProperty?.SetValue(namespaceMetadata, metadata.NamespaceName);
            namespaceMetadatasProperty?.SetValue(namespaceMetadata,
                HelperClass.ConvertList(namespaceMetadatasProperty.PropertyType.GetGenericArguments()[0],
                    metadata.Types.Select(t => new TypeMetadataMapper().MapDown(t, namespaceMetadatasProperty.PropertyType.GetGenericArguments()[0])).ToList()));

            return (NamespaceMetadataBase)namespaceMetadata;
        }
    }
}
