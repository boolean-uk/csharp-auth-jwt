using System.Runtime.Serialization;

namespace exercise.wwwapi.Enums
{
    public enum Role
    {
        [EnumMember(Value = "User")]
        User,
        [EnumMember(Value = "Administrator")]
        Administrator,
    }
}
