using exercise.wwwapi.Enums;
using exercise.wwwapi.Models.PureModels;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models.OutputModels
{
    public class UserDTO(ApplicationUser author)
    {
        public string Username { get; set; } = author.UserName;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; } = author.Role;
    }
}
