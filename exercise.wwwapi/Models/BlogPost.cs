namespace exercise.wwwapi.Models;

public class BlogPost
{
    public int? Id { get; set; }
    public required string Text { get; set; }
    public required int AuthorId { get; set; }

    public static BlogPost Create(int authorId, string text)
    {
        return new BlogPost { Text = text, AuthorId = authorId };
    }
}

public class BlogPostPost
{
    public required string Text { get; set; }
}
