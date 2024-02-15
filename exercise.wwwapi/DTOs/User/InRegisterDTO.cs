using exercise.wwwapi.Enums;
using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.DTOs.User
{
    public class InRegisterDTO
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
