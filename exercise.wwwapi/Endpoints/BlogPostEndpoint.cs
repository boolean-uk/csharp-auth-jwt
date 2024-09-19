using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransferObjects;
using exercise.wwwapi.DataViews;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        private static string _path = AppContext.BaseDirectory;
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {
            var blogposts = app.MapGroup("posts");
            blogposts.MapGet("/", GetPosts);
            blogposts.MapPost("/", CreatePost);
            blogposts.MapPut("/{id}", UpdatePost);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPosts(IRepository<BlogPost> service)
        {
            // get all posts, with Author as an inclusion, which is a User model
            var results = await service.GetAll(["Author"]);
            var resultDTOs = new List<BlogPostDTO>();
            foreach (var result in results) resultDTOs.Add(new BlogPostDTO(result));
            
            var payload = new Payload<IEnumerable<BlogPostDTO>>() { Data = resultDTOs };
            return TypedResults.Ok(payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreatePost(IRepository<BlogPost> service, BlogPostView view, ClaimsPrincipal user)
        {
            // get the id of the current user from the claims, that is stored in the token
            var userid = int.Parse(user.FindFirstValue(ClaimTypes.Sid)!);
            var model = new BlogPost()
            {
                Text = view.Text,
                AuthorId = userid
            };
            var result = await service.Create(["Author"], model);
            var resultDTO = new BlogPostDTO(result);

            var payload = new Payload<BlogPostDTO>() { Data = resultDTO };
            return TypedResults.Created(_path, payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UpdatePost(IRepository<BlogPost> service, int id, BlogPostUpdateView view, ClaimsPrincipal user)
        {
            // get the id of the current user from the claims, that is stored in the token
            var userid = int.Parse(user.FindFirstValue(ClaimTypes.Sid)!);
            if (view.AuthorId != 0) userid = view.AuthorId;

            var model = new BlogPost()
            {
                Id = id,
                Text = view.Text,
                AuthorId = userid
            };
            var result = await service.Update(["Author"], model);
            var resultDTO = new BlogPostDTO(model);

            var payload = new Payload<BlogPostDTO>() { Data = resultDTO };
            return TypedResults.Created(_path, payload);
        }
    }
}
