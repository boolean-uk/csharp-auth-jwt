namespace exercise.wwwapi.DTO
{
    public record BlogPostView(int Id, string Text);
    public record BlogPostViewComments(int Id, string Text, IEnumerable<CommentView> Comments);
    public record BlogPostPost(string Text);
    public record BlogPostPut(string Text);
}
