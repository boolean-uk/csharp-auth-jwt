using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } 
        public string Name { get; set; }

        public string Email { get; set; }
        public string HashedPassword { get; set; }

        public ICollection<BlogPost> Posts { get; set; }

        public ICollection<RelationStatus> AllFollowers { get; set; }
        public ICollection<BlogComment> Comments { get; set; }

    }
}
