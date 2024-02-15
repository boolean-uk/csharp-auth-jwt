using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models.PureModels
{
    [Table("entries")]
    public class Entry
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Required]
        [Column("content")]
        public string Content { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set;}

        [Column("author_id")]
        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }
    }
}
