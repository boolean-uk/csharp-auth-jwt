using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.Data.DTO
{
    public class LoginUserDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
