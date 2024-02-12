
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace auth.Model
{
    public enum UserRole
    {
        User,
        Admin
    }

    public class User : IdentityUser
    {
        public UserRole Role { get; set; }

        public ICollection <Comment> Comments { get; set; } = new List<Comment>();
    }
}
