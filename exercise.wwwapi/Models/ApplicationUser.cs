using Microsoft.AspNetCore.Identity;
using System.Data;
using exercise.wwwapi.Enum;
namespace exercise.wwwapi.Models
{
    public class ApplicationUser : IdentityUser
    {
       public Role Role { get; set; }
    }
}

