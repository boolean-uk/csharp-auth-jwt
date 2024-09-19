using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
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
        public List<string> Following { get; set; }
        public List<string> Followers { get; set; }
        public UserResponseDTO(User model)
        {
            Id = model.Id;
            Username = model.Username;
            PasswordHash = model.PasswordHash;
            // Checks if list elements are null, if so, return empty list
            Following = model.Following.Select(u => u.Followed?.Username ?? string.Empty).ToList() ?? new List<string>();
            Followers = model.Followers.Select(u => u.Follower?.Username ?? string.Empty).ToList() ?? new List<string>();
        }
    }

    [NotMapped]
    public class UserRequestDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
