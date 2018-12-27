namespace Data.DataMetadata
{
    public abstract class PropertyMetadataBase
    {
        public virtual string Name { get; set; }
        public virtual TypeMetadataBase TypeMetadata { get; set; }
    }
}
