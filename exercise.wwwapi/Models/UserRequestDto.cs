using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class UserRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
