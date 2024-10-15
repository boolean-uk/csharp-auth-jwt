using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models.DTOs
{
    [NotMapped]
    public class UserRequestDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    [NotMapped]
    public class UserResponseDto
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

}
