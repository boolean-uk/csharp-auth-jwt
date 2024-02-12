using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.Enums;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.Models
{
    [Table("users")]
    public class ApplicationUser : IdentityUser
    {
        [Column("user_id")]
        public int id;
        [Column("name")]
        public string Name;
        [Column("role")]
        public UserRole Role { get; set; }
    }
    
}
