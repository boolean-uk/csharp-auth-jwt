using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Column("Username")]
        public string Username { get; set; }

        [Column("PasswordHash")]
        public string PasswordHash { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        

    }
}
