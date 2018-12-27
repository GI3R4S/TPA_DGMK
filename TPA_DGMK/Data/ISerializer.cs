using Data.DataMetadata;

namespace Data
{
    public interface ISerializer
    {
        void Serialize(AssemblyMetadataBase data, string path);
        AssemblyMetadataBase Deserialize(string path);
    }
}
