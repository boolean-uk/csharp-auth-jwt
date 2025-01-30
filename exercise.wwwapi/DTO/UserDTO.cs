using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }
    }
}
