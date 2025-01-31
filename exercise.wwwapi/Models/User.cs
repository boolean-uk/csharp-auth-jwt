using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sodium;

namespace exercise.wwwapi.Models;

[Table("users")]
public class User
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("display_name")]
    [MaxLength(63)]
    public required string DisplayName { get; set; }

    [Column("username")]
    [MaxLength(63)]
    public required string Username { get; set; }

    [Column("email")]
    [MaxLength(255)]
    public required string Email { get; set; }
    
    [Column("password")]
    [MaxLength(255)]
    public string Password
    {
        get => _password;
        set => _password = HashPassword(value);
    }
    public IEnumerable<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    
    [NotMapped]
    private string _password = String.Empty;

    private string HashPassword(string password)
    {
        return PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Moderate).TrimEnd('\0');
    }

    public bool ValidatePassword(string password)
    {
        return PasswordHash.ArgonHashStringVerify(_password, password);
    }
}