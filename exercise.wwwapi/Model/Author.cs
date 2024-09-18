using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.Model
{

    public enum Role { USER, ADMIN }

    public class Author
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; } = Role.USER;
        public ICollection<UserRelationStatus> UserRelations { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; }
        public ICollection<BlogComment> BlogComments { get; set; }
    }
}
