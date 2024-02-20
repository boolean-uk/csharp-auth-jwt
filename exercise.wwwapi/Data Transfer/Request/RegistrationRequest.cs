using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.Data_Transfer.Request
{
    public class RegistrationRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        public Role Role { get; set; } = Role.User;  

    }
}
