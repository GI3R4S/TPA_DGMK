using Data;
using Data.DataMetadata;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.Serialization;
using ModelXml.XmlMetadata;

namespace ModelXml
{
    [Export(typeof(ISerializer))]
    public class XMLSerializer : ISerializer
    {
        public void Serialize(AssemblyMetadataBase data, string path)
        {
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
