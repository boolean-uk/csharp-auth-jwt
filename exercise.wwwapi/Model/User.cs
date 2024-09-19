using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Model
{
    [Table("User")]
    public class User
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("Username")]
        public string Username { get; set; }
        [Column("PasswordHash")]
        public string PasswordHash { get; set; }
    }
}
