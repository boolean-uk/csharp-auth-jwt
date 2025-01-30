using System.Security.Claims;
using AutoMapper;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints
{
    public static class PostsEndpoints
    {
        public static void ConfigurePosts(this WebApplication app)
        {
            var posts = app.MapGroup("/posts");

            posts.MapPost("/", Insert);
            posts.MapGet("/", GetAll);
            posts.MapPut("/{id}", Update);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> Insert(IRepository<Post> repository, IMapper mapper, PostRequestDTO post, ClaimsPrincipal claim)
        {
            try
            {
                Post insert = new Post()
                {
                    Text = post.Text,
                    UserId = claim.UserRealId()
                };

                await repository.Insert(insert);

                return TypedResults.Created($"https://localhost:7136/posts/{insert.Id}", insert);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetAll(IRepository<Post> repository, IMapper mapper)
        {
            try
            {
                IEnumerable<Post> posts = await repository.GetAll();

                var response = mapper.Map<IEnumerable<PostResponseDTO>>(posts);

                return TypedResults.Ok(new Payload<IEnumerable<PostResponseDTO>>() { data = response });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository<Post> repository, int id, PostRequestDTO post)
        {
            try
            {
                var target = await repository.GetById(id);

                if (target == null)
                    return Results.NotFound();
                if (post.Text != null)
                    target.Text = post.Text;

                await repository.Update(target);

                return TypedResults.Created($"https://localhost:7136/posts/{target.Id}", target);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }
    }
}
