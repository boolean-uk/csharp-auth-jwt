using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Model
{
    [NotMapped]
    public class UserRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
