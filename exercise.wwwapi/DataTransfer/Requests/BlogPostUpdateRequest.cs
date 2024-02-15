namespace exercise.wwwapi.DataTransfer.Requests
{
    public class BlogPostUpdateRequest
    {
        public required string Text { get; set; }
        public required string AuthorId { get; set; }
    }
}
