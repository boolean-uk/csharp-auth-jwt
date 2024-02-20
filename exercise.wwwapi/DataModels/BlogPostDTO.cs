namespace exercise.wwwapi.DataModels;

public class BlogPostDTO
{
    public int Id { get; set; }
    public string Text { get; set; }
    public static BlogPostDTO ToDTO(BlogPost post)
    {
        return new BlogPostDTO { Id = post.Id, Text = post.Text };
    }
}
