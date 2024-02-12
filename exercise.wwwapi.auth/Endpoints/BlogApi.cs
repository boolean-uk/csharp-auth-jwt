using System.Web.Http;

namespace exercise.wwwapi.auth.Endpoints
{
    public static class BlogApi
    {

        public static void ConfigureBlogApi(this WebApplication app)
        {
            var blogGroup = app.MapGroup("/blog");
            blogGroup.MapPost("/", CreateBlog);
        }


        [Authorize()]
        public static async Task<IResult> CreateBlog()
        {
            


            throw new NotImplementedException();
        }
    }
}
