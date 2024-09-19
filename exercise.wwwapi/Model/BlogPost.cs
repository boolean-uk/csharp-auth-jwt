namespace exercise.wwwapi.Model
{
    public class BlogPost
    {
        public int Id {  get; set; }
        public string Text { get; set; }
        public string authorId { get; set; }
        public Author Author { get; set; }

    }
}
