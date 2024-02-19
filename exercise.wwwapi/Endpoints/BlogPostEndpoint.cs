using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using workshop.webapi.DataModels;
using workshop.webapi.DataTransfer.Requests;
using workshop.webapi.Repository;

namespace workshop.webapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostsEndpoint(this WebApplication app)
        {

            var posts = app.MapGroup("posts");
            posts.MapGet("/", GetAll);
            posts.MapPost("/", AddPost).AddEndpointFilter(async (invocationContext, next) =>
            {
                var post = invocationContext.GetArgument<PostPostRequest>(1);

                if (string.IsNullOrEmpty(post.Title) || string.IsNullOrEmpty(post.Content))
                {
                    return Results.BadRequest("You must enter a Title AND Content");
                }
                return await next(invocationContext);
            }); ;
            posts.MapGet("/{id}", GetById);
            posts.MapPut("/{id}", Update);
            posts.MapDelete("/{id}", Delete);

        }
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> Delete(IRepository<BlogPost> repository, ClaimsPrincipal user, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Post with Id:{id}");
            }
            var result = await repository.Delete(entity);
            return result != null ? TypedResults.Ok(new { DateTime=DateTime.Now, User=user.Email(), Post=new { Title = result.Title, Content = result.Content }}) : TypedResults.BadRequest($"Post wasn't deleted");
        }
        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository<BlogPost> repository, int id, PostPatchRequest model, ClaimsPrincipal User)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Post with Id:{id}");
            }
            
            // Check if the user is an Admin or the Author of the post
            if (User.IsInRole("Admin") || User.UserId() == entity.AuthorId)
            {
                entity.Content = !string.IsNullOrEmpty(model.Content) ? model.Content : entity.Content;
                entity.Title = !string.IsNullOrEmpty(model.Title) ? model.Title : entity.Title;

                var result = await repository.Update(entity);

                return result != null
                    ? TypedResults.Ok(new { Title = result.Title, Content = result.Content })
                    : TypedResults.BadRequest("Couldn't save to the database?!");
            }
            else
            {
                return TypedResults.Unauthorized();
            }
        }

        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAll(IRepository<BlogPost> repository)
        {
            var entities = await repository.Get();
            List<Object> results = new List<Object>();
            foreach (var blogPost in entities)
            {
                results.Add(new { Id = blogPost.Id, Title = blogPost.Title, Content = blogPost.Content });
            }
            return TypedResults.Ok(results);
        }
        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetById(IRepository<BlogPost> repository, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Post with Id:{id}");
            }
            return TypedResults.Ok(new { Title = entity.Title, Content = entity.Content });
        }
        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> AddPost(IRepository<BlogPost> repository, PostPostRequest input, ClaimsPrincipal user)
        {
            var entity = new BlogPost() { Title = input.Title, Content = input.Content, AuthorId= user.UserId() };
            await repository.Insert(entity);
            return TypedResults.Created($"/{entity.Id}", new { Title = entity.Title, Content = entity.Content });

        }
    }
}
