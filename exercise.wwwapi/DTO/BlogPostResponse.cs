namespace exercise.wwwapi.DTO;

public class BlogPostResponse
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Author { get; set; }
}