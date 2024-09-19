using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels
{
    [Table("blogposts")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [ForeignKey("Author")]
        [Column("authorid")]
        public int AuthorId { get; set; }
        public User Author { get; set; }
    }
}
