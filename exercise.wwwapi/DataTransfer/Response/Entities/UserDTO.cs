using exercise.wwwapi.DataModels;
using exercise.wwwapi.Enums;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DataTransfer.Response.Entities
{
    public class UserDTO(User user)
    {
        public string? UserName { get; set; } = user.UserName;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole UserRole { get; set; } = user.UserRole;
    }
}
