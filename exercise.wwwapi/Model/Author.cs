using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.Model
{

    public enum Role { USER, ADMIN }

    public class Author
    {
        [Key]
        public string Id { get; set; } 
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; } = Role.USER;
        public ICollection<UserRelationStatus> UserRelations { get; set; } = new List<UserRelationStatus>();
        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
        public ICollection<BlogComment> BlogComments { get; set; } = new List<BlogComment>();
    }
}
