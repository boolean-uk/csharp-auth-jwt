using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs
{
    [NotMapped]
    public class UserRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }

        public required string Name { get; set; }
    }
}
