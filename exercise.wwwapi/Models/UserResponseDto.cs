using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

[NotMapped]
public class UserResponseDto
{
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string Email { get; set; }
}
