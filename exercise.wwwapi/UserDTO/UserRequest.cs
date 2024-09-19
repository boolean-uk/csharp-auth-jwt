using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.UserDTO
{
    [NotMapped]
    public class UserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
