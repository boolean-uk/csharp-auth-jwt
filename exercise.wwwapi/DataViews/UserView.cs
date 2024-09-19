using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataViews
{
    [NotMapped]
    public class UserView
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}