using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class UserResponseAuthorized
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public List<string> Follows { get; set; } = new List<string>();

    }
}
