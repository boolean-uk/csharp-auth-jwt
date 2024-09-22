namespace exercise.wwwapi.DTOs.BlogPosts
{
    public class GetBlogPostDTO
    {

        public string AuthorName { get; set; }
        public string Text { get; set; }
        public DateOnly Posted {  get; set; }
        // Add blog Comments later!!
    }
}
