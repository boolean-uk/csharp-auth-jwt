
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models
{
    [Table("author")]
    public class Author
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Blogpost> Posts { get; set; } = new List<Blogpost>();

    }
}
