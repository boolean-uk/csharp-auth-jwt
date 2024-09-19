using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("user")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("passwordhash")]
        public string PasswordHash { get; set; }

        List<BlogPost> Posts { get; set; } = new List<BlogPost>();
    }
}
