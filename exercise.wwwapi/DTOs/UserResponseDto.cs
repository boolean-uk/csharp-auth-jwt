using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs
{
    [NotMapped]
    public class UserResponseDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
