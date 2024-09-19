using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [NotMapped]
    public class FollowerDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }
}
