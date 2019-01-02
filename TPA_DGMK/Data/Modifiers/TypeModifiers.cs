using Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Data.Modifiers
{
    public class TypeModifiers
    {
        [Key]
        public int Id { get; set; }
        public AccessLevel? AccessLevel { get; set; }
        public SealedEnum? SealedEnum { get; set; }
        public AbstractEnum? AbstractEnum { get; set; }
        public StaticEnum? StaticEnum { get; set; }
    }
}
