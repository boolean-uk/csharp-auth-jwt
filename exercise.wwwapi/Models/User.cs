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

        [Column("password_hash")]
        public string PasswordHash { get; set; }

        public List<UserFollow> Following { get; set; } = new List<UserFollow>();
        public List<UserFollow> Followers { get; set; } = new List<UserFollow>();
    }
}
