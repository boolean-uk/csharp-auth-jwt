using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        public List<BlogPost> BlogPosts { get; set; } = [];
        public List<UserRelation> Following { get; set; } = [];
        public List<UserRelation> Followers { get; set; } = [];
        public List<Comment> Comments { get; set; } = [];
    }
}
