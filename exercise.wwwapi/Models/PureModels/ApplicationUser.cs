using exercise.wwwapi.Enums;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.Models.PureModels
{
    public class ApplicationUser : IdentityUser
    {
        public Role Role { get; set; }

        public ICollection<Entry> Entries { get; set; } = new List<Entry>();
    }
}
