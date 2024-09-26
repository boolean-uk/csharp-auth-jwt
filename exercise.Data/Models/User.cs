using exercise.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.Data.Models
{
    [Table("users")]
    public class User : IdentityUser, IEntity
    {
        [Column("posts")]
        public List<Post> Posts { get; set; }
        [Column("role")]
        public Role Role { get; set; }
    }
}
