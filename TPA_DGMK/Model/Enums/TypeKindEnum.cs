using System.Runtime.Serialization;

namespace Model.Enums
{
    public enum TypeKind
    {
        [EnumMember] ClassType, [EnumMember] EnumType, [EnumMember] InterfaceType, [EnumMember] StructType
    }
}
