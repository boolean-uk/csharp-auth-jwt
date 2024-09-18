using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("passwordhash")]
        public string PasswordHash { get; set; }
        //I take the easy way out this time
        public List<User> Follows { get; set; } = new List<User>();
        [JsonIgnore]
        public List<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }
}
