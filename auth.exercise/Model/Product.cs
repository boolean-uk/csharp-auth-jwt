using System.ComponentModel.DataAnnotations.Schema;

namespace auth.exercise.Model
{
    [Table("Products")]
    public class Product
    {
        [Column("id")]
        public int Id {get; set;}

        [Column("name")]
        public string Name {get; set;}

        [Column("price")]
        public int Price {get; set;}

        [Column("quantity")]
        public int Quantity {get; set;}
    }
}