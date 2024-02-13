using Microsoft.AspNetCore.Identity;
using exercise.wwwapi.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("app_user")]
    public class ApplicationUser : IdentityUser
    {
        [Column("role")]
        public UserRole Role { get; set; }
    }
}
