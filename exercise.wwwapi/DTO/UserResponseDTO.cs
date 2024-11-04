using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTO
{
    [NotMapped]
    public class UserResponseDTO
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }

    }
}
