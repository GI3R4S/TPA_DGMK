using System.Runtime.Serialization;

namespace Model
{
    public enum AbstractEnum
    {
       [EnumMember] NotAbstract, [EnumMember] Abstract
    }
}