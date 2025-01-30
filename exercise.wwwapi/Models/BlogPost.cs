using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{

    [Table ("post")]
    public class BlogPost
    {

        [Column("id")]
        public int Id { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("author_id")]
        [ForeignKey("User")]
        public string AuthorId { get; set; }

    }
}
