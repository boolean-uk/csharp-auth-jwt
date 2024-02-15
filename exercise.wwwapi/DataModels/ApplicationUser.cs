using exercise.wwwapi.Enums;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace exercise.wwwapi.DataModels
{
    public class ApplicationUser : IdentityUser
    {
        public Role Role { get; set; }
    }
}
