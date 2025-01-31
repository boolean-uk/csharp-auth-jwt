using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

[Table("blog_posts")]
public class BlogPost
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("title")]
    [MaxLength(255)]
    public required string Title { get; set; }
    [Column("content")]
    [MaxLength(4095)]
    public required string Content { get; set; }
    [Column("author_id")]
    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;
}