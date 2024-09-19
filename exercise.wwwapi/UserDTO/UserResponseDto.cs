using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.UserDTO
{
    [NotMapped]
    public class UserResponseDto
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
