using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    public enum Type
    {
        User,
        Admin
    }
    [Table("user")]
    public class User
    {
        
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}
