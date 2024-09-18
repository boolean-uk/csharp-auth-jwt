using exercise.wwwapi.Enums;
using exercise.wwwapi.Models.PureModels;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models.OutputModels
{
    /// <summary>
    /// Data transfer object for transfering information about a ApplicationUser out.
    /// </summary>
    /// <param name="user"> The user to be exported </param>
    public class UserDTO(ApplicationUser user)
    {
        public string Username { get; set; } = user.UserName;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; } = user.Role;
    }
}
