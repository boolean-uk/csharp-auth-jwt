using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTO.Requests
{
    public class AuthorPost
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
