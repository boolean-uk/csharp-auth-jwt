using exercise.wwwapi.Enums;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Model
{
    public class ApplicationUser :IdentityUser
    {
        public UserRole Role { get; set; }

        [JsonIgnore]
        public List<BlogPost> Posts { get; set; }
    }
}
