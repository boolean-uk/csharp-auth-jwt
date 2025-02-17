namespace exercise.wwwapi.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BlogPost
    {
      
        public int Id { get; set; }  

      
        public string Text { get; set; }

     
        public int AuthorId { get; set; } 

        public User Author { get; set; }  
    }
}

