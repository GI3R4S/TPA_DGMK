using Model;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Data_De_Serialization
{
    [Export(typeof(SerializerTemplate<>))]
    public class XMLSerializer<T> : SerializerTemplate<T>
    {
        public XMLSerializer() { }
        public override T Deserialize(string path)
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

        public override void Serialize(T data, string path)
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
    }
}
