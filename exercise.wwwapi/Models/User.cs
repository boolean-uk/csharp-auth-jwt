using System.ComponentModel.DataAnnotations.Schema;
using Sodium;

namespace exercise.wwwapi.Models;

[Table("users")]
public class User
{
    [Column("id")]
    public int Id { get; set; }

    [Column("display_name")]
    public string DisplayName { get; set; }

    [Column("username")]
    public string Username { get; set; }

    [Column("email")]
    public string Email { get; set; }
    
    [Column("password")]
    public string Password
    {
        get => _password;
        set => _password = HashPassword(value);
    }
    
    [NotMapped]
    private string _password;

    private string HashPassword(string password)
    {
        return PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Moderate).TrimEnd('\0');
    }

    public bool ValidatePassword(string password)
    {
        return PasswordHash.ArgonHashStringVerify(_password, password);
    }
}