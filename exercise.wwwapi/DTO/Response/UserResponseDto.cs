using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTO.Response
{
    [NotMapped]
    public class UserResponseDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = "user";
    }
}
