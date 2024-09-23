using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Model
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("passwordhash")]
        public string PasswordHash { get; set; }
    }
}
