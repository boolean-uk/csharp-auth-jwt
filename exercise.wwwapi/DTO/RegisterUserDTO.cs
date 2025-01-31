using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTO
{
    public class RegisterUserDTO
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
