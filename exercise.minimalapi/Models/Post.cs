using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.minimalapi.Models
{
    [Table("posts")]
    public class Post
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("author")]
        public required string AuthorId { get; set; }

        [Required]
        [Column("text")]
        public required string Text { get; set; }
    }
}
