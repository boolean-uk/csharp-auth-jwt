using exercise.wwwapi.Enum;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.Model
{
        public class ApplicationUser : IdentityUser
        {
            public Roles Role { get; set; }
        }
    
}
