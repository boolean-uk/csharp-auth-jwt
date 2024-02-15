using Microsoft.AspNetCore.Identity;
using System.Data;
using exercise.wwwapi.Enums;

namespace exercise.wwwapi.DataModels
{
    public class ApplicationUser : IdentityUser
    {
        public Role Role { get; set; }
    }
}
