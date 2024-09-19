using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.UserDTOs
{
    [NotMapped]
    public class UserResponseDto
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
