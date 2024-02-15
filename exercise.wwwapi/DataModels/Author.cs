using exercise.wwwapi.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DataModels
{
    [Table("authors")]
    public class Author : IdentityUser
    {
        [Required]
        [Column("role")]
        public Role Role {  get; set; }
       
    }
}
