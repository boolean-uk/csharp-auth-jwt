using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.DTO
{
    public class AuthorDTO
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public UserDTO User { get; set; }
    }
}
