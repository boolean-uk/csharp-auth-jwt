using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.DataTransfer.Request
{
    public class RegistrationRequest
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        public Role Role { get; set; }
    }
}
