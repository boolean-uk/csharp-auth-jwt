using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models
{
    [Table("user")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("passwordhash")]
        public string PasswordHash { get; set; }

        [Column("followingUserIds")]
        public List<int> FollowingUserIds { get; set; } = new List<int>();
        //public List<User> Following { get; set; } = new List<User>();
    }
}
