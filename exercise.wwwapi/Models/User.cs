using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

[Table("users")]
public class User
{
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    public required string Username { get; set; }

    [Column("passwordhash")]
    public required string PasswordHash { get; set; }

    [Column("email")]
    public required string Email { get; set; }

    [NotMapped]
    public virtual IEnumerable<BlogPost>? Posts { get; set; }

    [NotMapped]
    public virtual IEnumerable<UserFollow>? Following { get; set; }

    [NotMapped]
    public virtual IEnumerable<UserFollow>? FollowedBy { get; set; }
}
