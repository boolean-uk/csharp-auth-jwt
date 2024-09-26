using exercise.wwwapi.Enums;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.DataModels
{
    public class User :IdentityUser
    {

        public UserRole UserRole { get; set; }
    }
}
