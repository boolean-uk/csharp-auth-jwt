namespace exercise.wwwapi.Models
{
    public class Comment
    {

        public int Id { get; set; }

        public string text { get; set; }

        public string authorId { get; set; }   

        public Author Author { get; set; }

        public int BlogPostId { get; set; } 

        public BlogPost BlogPost { get; set; }


    }
}
