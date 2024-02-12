using exercise.minimalapi.Enums;
using Microsoft.AspNetCore.Identity;

namespace exercise.minimalapi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public UserRole Role { get; set; }
    }
}
