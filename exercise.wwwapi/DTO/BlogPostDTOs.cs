namespace exercise.wwwapi.DTO
{
    public record BlogPostView(int Id, string Text);
    public record BlogPostPost(string Text);
    public record BlogPostPut(string Text);
}
