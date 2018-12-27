using BusinessLogic.Mapping;
using BusinessLogic.Model;
using Data;
using Data.DataMetadata;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BusinessLogic
{
    [Export(typeof(LogicService))]
    public class LogicService
    {
        [ImportMany(typeof(ISerializer))]
        public IEnumerable<ISerializer> Serializer { get; set; }

        [Import(typeof(AssemblyMetadataBase))]
        public AssemblyMetadataBase AssemblyMetadata { get; set; }

        public void Serialize(AssemblyMetadata metadata, string path)
        {
            Serializer.ToList().FirstOrDefault()?.Serialize(AssemblyMetadataMapper.MapDown(metadata, AssemblyMetadata.GetType()), path);
        }

        public AssemblyMetadata Deserialize(string path)
        {
            return AssemblyMetadataMapper.MapUp(Serializer.ToList().FirstOrDefault()?.Deserialize(path));
        }
    }
}
