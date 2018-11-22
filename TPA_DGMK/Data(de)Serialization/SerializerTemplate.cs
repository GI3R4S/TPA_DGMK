namespace Data_De_Serialization
{
    public abstract class SerializerTemplate<T>
    {
        public SerializerTemplate() { }
        public abstract void Serialize(T data, string path);
        public abstract T Deserialize(string path);
    }
}
