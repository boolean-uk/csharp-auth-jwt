
using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.auth.Enums;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.auth.Models
{
    [Table("users")]
    public class ApplicationUser : IdentityUser
    {
        [Column("user_user_name")]
        public string UserMadeUserName { get; set; }
        [Column("role")]
        public UserRole Role { get; set; }

        // should add a relation to blog in ICollection
        public ICollection<Blog> BlogPosts { get; set; }
    }
}
