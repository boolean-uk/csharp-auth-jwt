using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTO
{
    [NotMapped]
    public class UserResponseDto
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
}
