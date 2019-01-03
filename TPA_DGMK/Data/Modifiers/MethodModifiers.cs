using Data.Enums;

namespace Data.Modifiers
{
    public class MethodModifiers
    {
        public int Id { get; set; }
        public AccessLevel? AccessLevel { get; set; }
        public AbstractEnum? AbstractEnum { get; set; }
        public StaticEnum? StaticEnum { get; set; }
        public VirtualEnum? VirtualEnum { get; set; }
    }
}
