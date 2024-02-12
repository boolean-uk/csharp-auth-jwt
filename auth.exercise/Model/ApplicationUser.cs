using Microsoft.AspNetCore.Identity;
using auth.exercise.Enums;

namespace auth.exercise.Model
{
    public class ApplicationUser : IdentityUser
    {
        public Roles Role { get; set;}
    }
}