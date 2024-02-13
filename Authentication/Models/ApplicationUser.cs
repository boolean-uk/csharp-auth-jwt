using Microsoft.AspNetCore.Identity;
using Authentication.Enums;

namespace Authentication.Model
{
    public class ApplicationUser : IdentityUser
    {
        public UserRole Role { get; set; }
    }
}