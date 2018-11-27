using Model;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Data_De_Serialization
{
    public class XMLSerializer : SerializerTemplate
    {
        public XMLSerializer() { }
        public override void Serialize<T>(T data, string path)
        {

            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                settings.CloseOutput = true;
                XmlWriter writer = XmlWriter.Create(fileStream, settings);
                DataContractSerializer dataContractSerializer;
                if (data is AssemblyMetadata)
                    dataContractSerializer = new DataContractSerializer(typeof(AssemblyMetadata));
                else
                    dataContractSerializer = new DataContractSerializer(typeof(T));
                dataContractSerializer.WriteObject(writer, data);
                writer.Close();
            }
        }
        public override T Deserialize<T>(string path)
        {
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T));
            T value;
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fileStream, new XmlDictionaryReaderQuotas { MaxDepth = int.MaxValue });
                try
                {
                    value = (T)dataContractSerializer.ReadObject(reader);
                }
                catch
                {
                    dataContractSerializer = new DataContractSerializer(typeof(AssemblyMetadata));
                    value = (T)dataContractSerializer.ReadObject(reader);
                }
                reader.Close();
            }
            return (T)(object)value;
        }
    }
}
