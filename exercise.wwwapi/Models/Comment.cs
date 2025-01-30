using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class Comment : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [ForeignKey(nameof(BlogPost))]
        public int BlogPostId { get; set; }

        public BlogPost BlogPost { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
