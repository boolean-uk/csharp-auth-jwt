using exercise.wwwapi.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels
{
    [Table("food")]
    public class Dinner : IFood
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("food_id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("cost")]
        public int Cost { get; set; }

        [Column("user_id")]
        public string ? UserId { get; set; } 



        //CAN ADD Chef with many to many relationship:

    }
}
