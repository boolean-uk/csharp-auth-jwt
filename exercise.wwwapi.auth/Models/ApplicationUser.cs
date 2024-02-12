using exercise.wwwapi.auth.Enums;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.auth.Models
{
    public class ApplicationUser : IdentityUser
    {

        public UserRole Role { get; set; }
    }
}
