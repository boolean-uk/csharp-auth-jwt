using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{

    public enum Role
    {
        User, Admin
    }
    [Table("applcations")]
    public class ApplicationUser : IdentityUser
    {

        public Role Role { get; set; }
    }
}
