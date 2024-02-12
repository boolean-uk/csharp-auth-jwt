using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public record PostPostPayload(string Text, int AuthorId);
    public record PutPostPayload(string Text, int AuthorId);
    [Table("posts")]
    public class Posts
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [Column("author_id")]
        public int AuthorId { get; set; }
        public User Author { get; set; }
    }
}
