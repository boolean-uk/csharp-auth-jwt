using Microsoft.AspNetCore.Identity;
using exercise_auth_jwt.Enum;

namespace exercise_auth_jwt.DataModels
{
    public class ApplicationUser : IdentityUser
    {
        
        public UserRole Role { get; set; }
    }
}
