using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.ViewModels
{
    [NotMapped]
    public class UserCreate
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
