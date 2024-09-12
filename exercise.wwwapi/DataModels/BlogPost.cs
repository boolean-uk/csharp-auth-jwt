using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels;

[Table("blogpost")]
public class BlogPost
{
    [Column("id")]
    public int Id { get; set; }
    [Column("text")]
    public string Text { get; set; }
    [Column("author_id")]
    public string UserId { get; set; }
    public User User { get; set; }
}
