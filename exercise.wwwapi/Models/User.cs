using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }


        [Column("username")]
        public string Username { get; set; }


        [Column("passwordhash")]
        public string PasswordHash { get; set; }


        [Column("email")]
        public string Email { get; set; }

        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    }
}
