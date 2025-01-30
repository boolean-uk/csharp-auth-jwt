namespace exercise.wwwapi.DTO
{
    public record CommentPost(string Text);
    public record CommentView(int Id, string Text, UserSimple User);
}
