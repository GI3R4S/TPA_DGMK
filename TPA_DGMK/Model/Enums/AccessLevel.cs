using System.Runtime.Serialization;

namespace Model
{
    public enum AccessLevel
    {
        [EnumMember] IsPublic, [EnumMember] IsProtected, [EnumMember] IsProtectedInternal, [EnumMember] IsPrivate
    }
}