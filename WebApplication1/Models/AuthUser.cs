using Microsoft.AspNetCore.Identity;
using WebApplication1.Enums;


namespace WebApplication1.Models
{
    public class AuthUser : IdentityUser
    {
        public UserRole Role {  get; set; }
    }
}
