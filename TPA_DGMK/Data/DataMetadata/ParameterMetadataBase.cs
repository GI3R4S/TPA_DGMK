namespace Data.DataMetadata
{
    public abstract class ParameterMetadataBase
    {
        public virtual string Name { get; set; }
        public virtual TypeMetadataBase TypeMetadata { get; set; }
    }
}
