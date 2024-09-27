using exercise.wwwapi.Enums;
using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.DataTransfer.Requests.User
{
    public class RegistrationRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Username { get { return Email; } set { } }

        [Required]
        public string? Password { get; set; }
        //Enums has integer values as input, so we can use int here. I have spent too much time figuring out why how I can use string. Now I am just going to use int.
        public int Role { get; set; }
    }
}
