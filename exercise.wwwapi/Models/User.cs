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

        [Column("passwordhash")]
        public string PasswordHash { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
