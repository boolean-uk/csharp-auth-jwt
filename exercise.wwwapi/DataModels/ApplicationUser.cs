using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.Models
{
    public enum Role
    {
        Admin,
        User
    }

    public class ApplicationUser : IdentityUser
    {
        public Role Role { get; set; }
    }
}
