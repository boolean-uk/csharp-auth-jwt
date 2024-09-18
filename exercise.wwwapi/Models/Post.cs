using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class Post
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("text")]
        public string Text { get; set; } = "";
        [Column("author_id")]
        [ForeignKey("ApplicationUser")]
        public string AuthorId { get; set; } = "";

        public Post Update(PostInput input)
        {
            this.Text = input.Text;
            return this;
        }
    }

    public struct PostInput
    {
        public string Text { get; set; }
    }
}
