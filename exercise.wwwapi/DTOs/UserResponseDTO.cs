using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.DTOs;

namespace exercise.wwwapi.Models
{
    public class UserResponseDTO
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
}
