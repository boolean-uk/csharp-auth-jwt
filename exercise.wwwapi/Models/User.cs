using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.Models
{
    public class User
    {
        [Key]
        public int Id { get ; set; }
        public string Name { get; set; }

        public string HashedPassword { get; set; }

        public ICollection<BlogPost> Posts { get; set; }

        public ICollection<RelationStatus> AllFollowers { get; set; }
        public ICollection<BlogComment> Comments { get; set; }

    }
}
