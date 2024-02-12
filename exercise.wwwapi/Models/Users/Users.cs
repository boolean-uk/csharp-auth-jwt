using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.Data;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.Models
{
    [Table("users")]
    public class Users : IdentityUser
    {
        //inherits from IdentityUser which includes standard user properties like username, email, pwd
        public UserRole Role { get; set; }
        public ICollection<Posts> Posts { get; set; }
    }
}