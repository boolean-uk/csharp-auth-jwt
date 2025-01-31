using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Model
{
    [NotMapped]
    public class UserRequestDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
