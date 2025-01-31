using System.Security.Claims;
using AutoMapper;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.EndPoints;

public static class SecureApi
{
    public static void ConfigureSecureApi(this WebApplication app)
    {
        app.MapPost("/posts", CreatePost);
        app.MapPut("/posts", UpdatePost);
        app.MapGet("/posts", GetPosts);
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private static async Task<Results<Ok<IEnumerable<BlogPost>>, UnauthorizedHttpResult>> GetPosts(
        IRepository<User> userRepo,
        IRepository<BlogPost> blogRepo,
        ClaimsPrincipal user
    )
    {
        return TypedResults.Ok(await blogRepo.GetAll());
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private static async Task<
        Results<Ok<BlogPost>, UnauthorizedHttpResult, BadRequest<string>>
    > CreatePost(
        IRepository<User> userRepo,
        IRepository<BlogPost> blogRepo,
        ClaimsPrincipal user,
        BlogPost blogpost
    )
    {
        var id = user.UserRealId();
        if (id == null)
        {
            return TypedResults.BadRequest("Error with user id");
        }

        var post = BlogPost.Create(id ?? 0, blogpost.Text);
        var result = await blogRepo.Insert(blogpost);
        if (result == null)
        {
            return TypedResults.BadRequest("Something went wrong :)");
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private static async Task<
        Results<Ok<BlogPost>, UnauthorizedHttpResult, BadRequest<string>>
    > UpdatePost(
        IRepository<User> userRepo,
        IRepository<BlogPost> blogRepo,
        IMapper mapper,
        ClaimsPrincipal user,
        int blogId,
        BlogPostPost updated
    )
    {
        if (blogId != user.UserRealId())
        {
            return TypedResults.Unauthorized();
        }
        var result = await blogRepo.Update(mapper, blogId, updated);
        if (result == null)
        {
            return TypedResults.BadRequest("Try again with a better payload please");
        }
        return TypedResults.Ok(result);
    }
}
