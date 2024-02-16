using exercise.wwwapi.Enums;
using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.DataTransfer.Request
{
    public class RegistrationRequest
    {

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Username { get { return this.Email; } set { } }

        [Required]
        public string? Password { get; set; }

        public Role role { get; set; } = Role.User;

    }
}
