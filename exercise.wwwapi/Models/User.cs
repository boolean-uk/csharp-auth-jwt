using System.ComponentModel.DataAnnotations.Schema;
using api_cinema_challenge.Models.Interfaces;

namespace exercise.wwwapi.Models
{
    [Table("users")]
    public class User : ICustomModel
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("passwordhash")]
        public string PasswordHash { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("role")]
        public string Role { get; set; }

        // Navigation properties 
        public List<BlogPost> BlogPosts { get; set; }
        public List<UserFollows> Followers{ get; set; }
        public List<UserFollows> Followings { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
