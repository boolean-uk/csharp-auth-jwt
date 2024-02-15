using exercise.wwwapi.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("application_user")]
    public class ApplicationUser : IdentityUser
    {
        [Column("user_id")]
        public int id;

        /*
        [Column("user_name")]
        public string UserName;*/ 

        [Column("user_role")]
        public Role Role { get; set; }
    }
}
