using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace exercise.wwwapi.auth.Models
{
    [Table("blogs")]
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        [Column("author_name")]
        public string AuthorName { get; set; }  
        [Column("title")]
        public string Title { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("last_updated")]
        public DateTime LastUpdated { get; set; }

        [Column("author_id")]
        public string AuthorId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}