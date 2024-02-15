using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models.PureModels
{
    /// <summary>
    /// Object that contains all relevant information about a specific text entry. 
    /// </summary>
    [Table("entries")]
    public class Entry
    {
        /// <summary>
        /// Primary key and identifier of the Entry, unique integer value.
        /// </summary>
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Required]
        [Column("content")]
        public string Content { get; set; }

        /// <summary>
        /// The DateTime when the Entry object was first created/submitted to the database.
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The last time any chagnes was performed on the Entry object.
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set;}

        /// <summary>
        /// The ID of the ApplicationUser that submitted/created the Entry
        /// </summary>
        [Column("author_id")]
        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; }

        /// <summary>
        /// The ApplicationUser object that submitted/created the Entry.
        /// </summary>
        public ApplicationUser Author { get; set; }
    }
}
