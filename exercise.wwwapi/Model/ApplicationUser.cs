using exercise.wwwapi.Enums;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.Model
{
    public class ApplicationUser : IdentityUser
    {
        public Role Role { get; set; }               
    }
}
