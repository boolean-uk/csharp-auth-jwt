using exercise.wwwapi.Enums;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.Models.PureModels
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Enum value indicating access level and responsibility.
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Collection of entries that was written/submitted by the user.
        /// </summary>
        public ICollection<Entry> Entries { get; set; } = new List<Entry>();
    }
}
