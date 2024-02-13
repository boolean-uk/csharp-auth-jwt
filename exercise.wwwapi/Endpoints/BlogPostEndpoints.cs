using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransferObjects;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoints
    {
        public static void ConfigureBlogpostEndpoints(this WebApplication app)
        {
            var Blog = app.MapGroup("posts");
            Blog.MapGet("/", GetAllPosts);
            Blog.MapPost("/", MakeAPost);
            Blog.MapPut("/{blogpostId}", EditPost);
        }

        public static async Task<IResult> GetAllPosts(IRepository repository)
        {
            var results = await repository.GetAllPosts();
            var payload = new Payload<IEnumerable<BlogPost>>() { data = results };

            return TypedResults.Ok(payload);
        }

        public static async Task<IResult> MakeAPost(IRepository repository, BlogPostPayload postPayload)
        {
            var result = await repository.MakeAPost(postPayload.text, postPayload.authorId);
            var payload = new Payload<BlogPost>() { data = result };

            return TypedResults.Ok(payload);
        }

        public static async Task<IResult> EditPost(IRepository repository, BlogPostUpdateData updateData)
        {
            var result = await repository.EditPost(updateData.postId ,updateData.text, updateData.authorId);
            var payload = new Payload<BlogPost>() { data = result };

            return TypedResults.Ok(payload);
        }




    }

}
