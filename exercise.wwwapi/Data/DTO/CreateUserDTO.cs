using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.Data.DTO
{
    public class CreateUserDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
