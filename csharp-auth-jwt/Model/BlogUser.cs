using csharp_auth_jwt.Model.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace csharp_auth_jwt.Model
{
    [Table("blogUsers")]
    public class BlogUser : IdentityUser
    {
        [Column("role")]
        public BlogUserRole Role { get; set; }
    }
}
