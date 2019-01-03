using Data;
using Data.DataMetadata;
using System.ComponentModel.Composition;
using ModelXml.XmlMetadata;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace ModelXml
{
    [Export(typeof(ISerializer))]
    public class XMLSerializer : ISerializer
    {
        public void Serialize(AssemblyMetadataBase data, string path)
        {
            AssemblyMetadataXml assembly = data as AssemblyMetadataXml;
            string jsonString = JsonConvert.SerializeObject(assembly, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
            XDocument document = JsonConvert.DeserializeXNode(jsonString, "Root", true);
            document.Save(path);
        }
        public AssemblyMetadataBase Deserialize(string path)
        {
            AssemblyMetadataXml metadata;
            XDocument document = XDocument.Load(path);
            string jsonString = JsonConvert.SerializeXNode(document, Formatting.Indented, true);
            jsonString = jsonString.Remove(0, 58);
            metadata = JsonConvert.DeserializeObject<AssemblyMetadataXml>(jsonString, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None
            });
            return metadata;
        }
    }
}
