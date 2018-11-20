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
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            T value;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas { MaxDepth = int.MaxValue });
                try
                {
                    value = (T)ser.ReadObject(reader);
                }
                catch
                {
                    ser = new DataContractSerializer(typeof(AssemblyMetadata));
                    value = (T)ser.ReadObject(reader);
                }
                reader.Close();
            }
            return (T)(object)value;
        }

        public override void Serialize(T data, string path)
        {

            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                settings.CloseOutput = true;
                XmlWriter writer = XmlWriter.Create(fs, settings);
                DataContractSerializer ser;
                if (data is AssemblyMetadata)
                    ser = new DataContractSerializer(typeof(AssemblyMetadata));
                else
                    ser = new DataContractSerializer(typeof(T));
                ser.WriteObject(writer, data);
                writer.Close();
            }
        }
    }
}
