using exercise.wwwapi.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace exercise.wwwapi.Models
{
    public class Author
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; } = Role.USER;
        public ICollection<UserRelations> UserRelations { get; set; } = new List<UserRelations>();
        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
        public ICollection<BlogComments> BlogComments { get; set; } = new List<BlogComments>();

    }
}
