using BusinessLogic.Model;
using Data.DataMetadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BusinessLogic.Mapping
{
    public class AssemblyMetadataMapper
    {
        public static AssemblyMetadataBase MapToSerialize(AssemblyMetadata metadata, Type assemblyMetadataType)
        {
            object assemblyMetadata = Activator.CreateInstance(assemblyMetadataType);
            PropertyInfo nameProperty = assemblyMetadataType.GetProperty("Name");
            PropertyInfo namespaceMetadataProperty = assemblyMetadataType.GetProperty("Namespaces", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            nameProperty?.SetValue(assemblyMetadata, metadata.Name);
            namespaceMetadataProperty?.SetValue(assemblyMetadata, ConvertionUtilities.ConvertList(namespaceMetadataProperty.PropertyType.GetGenericArguments()[0],
                metadata.Namespaces.Select(n => new NamespaceMetadataMapper().MapToSerialize(n, namespaceMetadataProperty.PropertyType.GetGenericArguments()[0])).ToList()));
            return (AssemblyMetadataBase)assemblyMetadata;
        }

        public static AssemblyMetadata MapToDeserialize(AssemblyMetadataBase metadata)
        {
            AssemblyMetadata assemblyModel = new AssemblyMetadata();
            Type type = metadata.GetType();
            assemblyModel.Name = metadata.Name;
            PropertyInfo namespaceMetadatasProperty = type.GetProperty("Namespaces", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            List<NamespaceMetadataBase> namespaces = (List<NamespaceMetadataBase>)ConvertionUtilities.ConvertList(typeof(NamespaceMetadataBase), (IList)namespaceMetadatasProperty?.GetValue(metadata));
            if (namespaces != null)
                assemblyModel.Namespaces = namespaces.Select(n => new NamespaceMetadataMapper().MapToDeserialize(n)).ToList();
            return assemblyModel;
        }
    }
}
