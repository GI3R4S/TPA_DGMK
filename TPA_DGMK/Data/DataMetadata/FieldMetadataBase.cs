using Data.Enums;

namespace Data.DataMetadata
{
    public abstract class FieldMetadataBase
    {
        public virtual string Name { get; set; }
        public virtual TypeMetadataBase TypeMetadata { get; set; }
        public virtual AccessLevel AccessLevel { get; set; }
        public virtual bool IsStatic { get; set; }
    }
}
