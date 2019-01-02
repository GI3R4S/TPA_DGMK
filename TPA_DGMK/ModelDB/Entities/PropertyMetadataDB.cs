using Data.DataMetadata;

namespace ModelDB.Entities
{
    public class PropertyMetadataDB : PropertyMetadataBase
    {
        public int Id { get; set; }
        public override string Name { get; set; }
        public new TypeMetadataDB TypeMetadata { get; set; }
    }
}
