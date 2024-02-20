using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DataModels
{
    [Table("cars")]
    public class Car
    {
        [Column("id"), JsonIgnore]
        public int Id { get; set; }
        [Column("make")]
        public string Make { get; set; }
        [Column("model")]
        public string Model { get; set; }
    }
}
