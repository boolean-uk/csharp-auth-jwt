using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransfer.Response;
using exercise.wwwapi.DataTransfer.Response.Entities;
using exercise.wwwapi.Repository;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {
            var blogPosts = app.MapGroup("blogposts");
            blogPosts.MapGet("/", GetAll);
        }

        public static async Task<IResult> GetAll(IRepository<BlogPost> repository)
        {
            IEnumerable<BlogPost> results = await repository.GetAll();
            List<BlogPostDTO> resultDTOs = new List<BlogPostDTO>();
            foreach (BlogPost blogPost in results)
            {
                resultDTOs.Add(new BlogPostDTO(blogPost));
            }
            return TypedResults.Ok(new Payload<List<BlogPostDTO>>(resultDTOs));
        }
    }
}
