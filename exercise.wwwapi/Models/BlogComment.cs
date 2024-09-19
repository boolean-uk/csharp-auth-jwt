using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class BlogComment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Blog")]

        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

        public string Comment { get; set; }
    }
}
