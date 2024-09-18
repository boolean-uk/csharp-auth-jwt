using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs
{
    [NotMapped]
    public class UserRequestDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
