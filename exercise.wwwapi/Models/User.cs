using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.Models
{
    public class User : IdentityUser
    {
        public string Id { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        public List<BlogPost> BlogPosts { get; set; } = [];
    }
}
