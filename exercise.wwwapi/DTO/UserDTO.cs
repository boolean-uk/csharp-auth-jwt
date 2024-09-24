using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.DTO
{
    [NotMapped]
    public class UserDTO
    {
        public int ID { get; set; }
        public string Username { get; set; }
    }
}
