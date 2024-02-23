using exercise.wwwapi.Enum;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace exercise.wwwapi.DataTransfer.Request
{
    public class RegisterUser
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Username { get { return this.Email; } set { } }

        [Required]
        public string? Password { get; set; }

        public Role Role { get; set; } = Role.User;
    }
}
