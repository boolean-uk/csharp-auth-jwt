using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.Models
{
    public class Authentication
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

        public bool IsValid()
        {
            return true;
        }
    }

    public class AuthenticationResponse
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }  
    }

    public class Registration
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
