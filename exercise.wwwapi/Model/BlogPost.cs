using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Model
{
    [Table("posts")]
    public class BlogPost
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("Text")]
        public string Text { get; set; }

        [Column("Author")]
        public string Author { get; set; }

        [Column("Created_At")]
        public DateTime createdAt { get; set; }
    }
}
