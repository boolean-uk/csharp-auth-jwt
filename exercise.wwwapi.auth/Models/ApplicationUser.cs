using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.auth.Enums;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.auth.Models
{
    [Table("users")]
    public class ApplicationUser : IdentityUser
    {
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("email")]
        public string Email { get; set; }
        public UserRole Role { get; set; }
        [Column("role")]
        public UserRole Role { get; set; }
    }
}
