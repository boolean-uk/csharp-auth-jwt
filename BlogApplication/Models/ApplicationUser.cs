using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using BlogApplication.Enums;

namespace BlogApplication.Models
{

 
    [Table("applicationusers")]
     
    public class ApplicationUser : IdentityUser
    {
 
        [Column("role")]
        public UserRole Role { get; set; }

        // public ICollection<BlogPost> BlogPosts { get; set; }


    }
    
}
