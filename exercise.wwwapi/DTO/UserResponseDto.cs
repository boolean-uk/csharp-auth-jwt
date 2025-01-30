using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTO
{
    [NotMapped]
    public class UserResponseDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        
    }
}
