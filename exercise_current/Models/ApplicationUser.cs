using Microsoft.AspNetCore.Identity;
using exercise.wwwapi.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("app_user")]
    public class ApplicationUser : IdentityUser
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("role")]
        public UserRole Role { get; set; }
    }
}
