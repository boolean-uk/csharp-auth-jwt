using System.Runtime.Serialization;

namespace exercise.wwwapi.Enums
{
    /// <summary>
    /// Indication of the account responsibility level, either "User"/0 for regular end-user, or "Administrator"/1 for the people responsible for moderating the site.
    /// </summary>
    public enum Role
    {
        [EnumMember(Value = "User")]
        User,
        [EnumMember(Value = "Administrator")]
        Administrator,
    }
}
