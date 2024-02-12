using Microsoft.AspNetCore.Identity;
using exercise.wwwapi.Enums;
namespace exercise.wwwapi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public UserRole Role { get; set; }
    }
}
