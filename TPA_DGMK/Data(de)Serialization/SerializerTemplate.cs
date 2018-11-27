namespace Data_De_Serialization
{
    public abstract class SerializerTemplate
    {
        public SerializerTemplate() { }
        public abstract void Serialize <T>(T data, string path);
        public abstract T Deserialize <T>(string path);
    }
}
