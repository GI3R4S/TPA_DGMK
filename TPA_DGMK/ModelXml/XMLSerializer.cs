using Data;
using Data.DataMetadata;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.Serialization;
using ModelXml.XmlMetadata;
using System.Xml;
using System.Collections.Generic;
using System;

namespace ModelXml
{
    [Export(typeof(ISerializer))]
    public class XMLSerializer : ISerializer
    {
        public void Serialize(AssemblyMetadataBase data, string path)
        {
            //using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
            //    settings.CloseOutput = true;
            //    XmlWriter writer = XmlWriter.Create(fileStream, settings);
            //    DataContractSerializer dataContractSerializer;
            //    if (data is AssemblyMetadataXml)
            //        dataContractSerializer = new DataContractSerializer(typeof(AssemblyMetadataXml));
            //    else
            //    {
            //        var list = new List<Type>
            //        {
            //            typeof(TypeMetadataXml),
            //            typeof(NamespaceMetadataXml),
            //            typeof(ParameterMetadataXml),
            //            typeof(PropertyMetadataXml),
            //            typeof(FieldMetadataXml),
            //            typeof(MethodMetadataXml)
            //        };
            //        dataContractSerializer = new DataContractSerializer(data.GetType(), list);
            //    }
            //    dataContractSerializer.WriteObject(writer, data);
            //    writer.Close();
            //}

            AssemblyMetadataXml assembly = (AssemblyMetadataXml)data;
            DataContractSerializer dataContractSerializer =
                new DataContractSerializer(typeof(AssemblyMetadataXml));

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                dataContractSerializer.WriteObject(fileStream, assembly);
            }
        }
        public AssemblyMetadataBase Deserialize(string path)
        {
            AssemblyMetadataXml metadata;
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(AssemblyMetadataXml));
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                metadata = (AssemblyMetadataXml)dataContractSerializer.ReadObject(fileStream);
            }
            return metadata;
        }
    }
}
