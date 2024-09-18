using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class UserRequestDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
