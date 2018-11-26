using System.Runtime.Serialization;

namespace Model
{
    public enum SealedEnum
    {
        [EnumMember] Sealed, [EnumMember] NotSealed
    }
}
