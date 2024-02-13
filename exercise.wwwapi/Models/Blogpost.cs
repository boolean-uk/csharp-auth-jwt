using System.ComponentModel.DataAnnotations.Schema;


namespace exercise.wwwapi.Models
{
    [Table("posts")]
    public class Blogpost
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("text")]
        public string Text { get; set; }
        [Column("authorId")]
        public int AuthorId { get; set; }
        
        [Column("author")]
        public Author Author { get; set; }

    }
}
