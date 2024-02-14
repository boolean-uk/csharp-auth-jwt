using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataModels.Requests;
using exercise.wwwapi.DataModels.Response;

namespace exercise.wwwapi.Services
{
    public static class BlogPostDtoManager
    {
        public static BlogPostOutput Convert(BlogPost post) => new BlogPostOutput
        {
            Id = post.Id,
            Text = post.Text,
            AuthorId = post.AuthorId
        };

        public static IEnumerable<BlogPostOutput> Convert(IEnumerable<BlogPost> blogPosts) => blogPosts.Select(Convert);

        public static BlogPost Convert(BlogPostInput input) => new BlogPost
        {
            Text = input.Text
        };
    }
}
