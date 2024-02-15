namespace exercise.wwwapi.DTO.Request
{
    public class BlogPostRequest
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public DateTime createdAt { get; set; }
    }
}
