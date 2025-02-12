using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("User")]
    public class User
    {
        [Column("userid")]
        public int userId { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("passwordhash")]
        public string PasswordHash { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [NotMapped]
        public virtual List<Post> Posts { get; set; }
        [NotMapped]
        public virtual List<Comment> Comments { get; set; }
    }
}