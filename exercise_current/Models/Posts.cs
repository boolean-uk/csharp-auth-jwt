using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public record PostPostPayload(string Text);
    public record PutPostPayload(string Text);
    [Table("posts")]
    public class Posts
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [Column("author_id")]
        public string AuthorId { get; set; }
        //public ApplicationUser Author { get; set; }
    }
}
