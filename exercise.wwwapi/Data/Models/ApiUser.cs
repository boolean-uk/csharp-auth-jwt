using exercise.wwwapi.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Data.Models
{
    public class ApiUser : IdentityUser
    {
        [Required]
        [Column("role")]
        public Role Role { get; set; }
    }
}
