using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }
}
