using Data;
using Data.DataMetadata;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.Serialization;
using ModelXml.XmlMetadata;
using System.Xml;

namespace ModelXml
{
    [Export(typeof(ISerializer))]
    public class XMLSerializer : ISerializer
    {
        public void Serialize(AssemblyMetadataBase data, string path)
        {
            AssemblyMetadataXml assembly = (AssemblyMetadataXml)data;
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(AssemblyMetadataXml));
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (var writer = XmlWriter.Create(fileStream, settings))
                    dataContractSerializer.WriteObject(writer, assembly);
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
