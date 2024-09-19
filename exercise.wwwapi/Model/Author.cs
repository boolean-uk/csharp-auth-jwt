using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Model
{
    public class Author
    {
        [Column("id")]
        public string Id { get; set; }
        [Column("username")]
        public string UserName { get; set; }
        [Column("passwordhash")]
        public string PasswordHash { get; set; }
        public List<BlogPost> Posts { get; set; }   
        
    }
}
