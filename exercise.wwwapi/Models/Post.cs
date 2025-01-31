using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models
{
    [Table("posts")]
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; }

        [ForeignKey("Author_Id")]
        public int AuthorId { get; set; }
        public User Author { get; set; }

    }
}
