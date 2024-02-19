using exercise.wwwapi.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column("role")]
        public Role Role { get; set; }
    }
}
