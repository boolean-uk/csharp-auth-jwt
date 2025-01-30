using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("users")]
    public class User : BaseEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("passwordhash")]
        public string PasswordHash { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("role")]
        public string Role { get; set; } = "user";
        public IEnumerable<BlogPost> BlogPosts { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
