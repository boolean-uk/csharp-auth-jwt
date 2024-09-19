using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs
{
    [NotMapped]
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public UserResponseDTO(User model)
        {
            Id = model.Id;
            Username = model.Username;
            PasswordHash = model.PasswordHash;
        }
    }

    [NotMapped]
    public class UserRequestDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
