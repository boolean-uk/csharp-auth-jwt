using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class Comment
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        [Column("Text")]
        public string Text { get; set; }

    }
}
