using Data.DataMetadata;

namespace ModelDB.Entities
{
    public class ParameterMetadataDB : ParameterMetadataBase
    {
        public int Id { get; set; }
        public override string Name { get; set; }
        public new TypeMetadataDB TypeMetadata { get; set; }
    }
}
