using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class Author : BaseModel
    {
        [Key]
        public Guid Id { get; set; }

        public string DisplayName { get; set; }
        public string Bio { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
