using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.Enums;

namespace exercise.wwwapi.Models
{
    [Table("application_user")]
    public class ApplicationUser : IdentityUser
    {
        [Column("user_role")]
        public UserRole Role { get; set; }
    }
}
