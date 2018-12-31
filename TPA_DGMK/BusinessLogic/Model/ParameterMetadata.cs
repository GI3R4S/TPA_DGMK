namespace BusinessLogic.Model
{
    public class ParameterMetadata
    {
        public string Name { get; set; }
        public TypeMetadata TypeMetadata { get; set; }

        public ParameterMetadata(string name, TypeMetadata typeMetadata)
        {
            Name = name;
            TypeMetadata = typeMetadata;
        }

        public ParameterMetadata() { }
    }
}
