using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.Models
{
    public class AppUser : IdentityUser
    {
        public Role Role { get; set; }
    }
}
