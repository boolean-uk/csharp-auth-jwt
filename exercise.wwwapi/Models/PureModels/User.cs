using exercise.wwwapi.Enums;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.Models.PureModels
{
    public class User : IdentityUser
    {
        public Role Role { get; set; }
    }
}
