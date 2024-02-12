using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace exercise.wwwapi.auth.Models
{
    [Table("blogs")]
    public class Blog
    {
        [Column("title")]
        public string Title { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("last_updated")]
        public DateTime LastUpdated { get; set; }
    }
}