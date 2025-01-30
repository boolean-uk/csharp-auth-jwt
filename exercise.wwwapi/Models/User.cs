using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public class User : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public Author Author { get; set; }

        public ICollection<UserFollower> Followers { get; set; } = new List<UserFollower>();
        public ICollection<UserFollower> Followees { get; set; } = new List<UserFollower>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
