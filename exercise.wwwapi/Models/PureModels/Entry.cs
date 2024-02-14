using System.ComponentModel.DataAnnotations;

namespace exercise.wwwapi.Models.PureModels
{
    public class Entry
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string PostText { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set;}

        public int AuthorId { get; set; }

        public User Author { get; set; }
    }
}
