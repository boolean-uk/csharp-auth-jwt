using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("cars")]
    public class Car
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("make")]
        public string Make { get; set; }

        [Column("model")]
        public string Model { get; set; }
    }
}
