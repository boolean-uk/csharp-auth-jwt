using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Model
{
    [NotMapped]
    public class UserResponse
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
