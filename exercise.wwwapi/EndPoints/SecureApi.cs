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
        app.MapPost("/users/follow/{userId}", FollowUser);
        app.MapPost("/users/unfollow/{userId}", UnFollowUser);
        app.MapGet("/posts/following", GetFollowingPosts);
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
        BlogPostPost blogpost
    )
    {
        var id = user.UserRealId();
        if (id == null)
        {
            return TypedResults.BadRequest("Error with user id");
        }

        var post = BlogPost.Create(id ?? 0, blogpost.Text);
        var result = await blogRepo.Insert(post);
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
        var source = await blogRepo.GetById(blogId);
        if (source == null)
            return TypedResults.BadRequest("No blog with given id");
        if (source.AuthorId != user.UserRealId())
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

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<
        Results<Ok<UserFollow>, UnauthorizedHttpResult, BadRequest<string>>
    > FollowUser(
        IRepository<UserFollow> followRepo,
        IRepository<User> userRepo,
        ClaimsPrincipal user,
        int followUserId
    )
    {
        var id = user.UserRealId();
        if (id == null)
        {
            return TypedResults.BadRequest("You don't exist?");
        }
        if (!await userRepo.Exists(followUserId))
        {
            return TypedResults.BadRequest("User to follow does not exist");
        }
        var followExists = await followRepo.GetBy(f =>
            f.FolloweeId == followUserId && f.FollowerId == id
        );
        if (followExists != null)
        {
            return TypedResults.BadRequest("Already following");
        }
        var follow = new UserFollow
        {
            FollowerId = user.UserRealId() ?? 0,
            FolloweeId = followUserId,
        };
        var result = await followRepo.Insert(follow);
        return TypedResults.Ok(result);
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<
        Results<Ok<UserFollow>, UnauthorizedHttpResult, BadRequest<string>>
    > UnFollowUser(
        IRepository<UserFollow> followRepo,
        IRepository<User> userRepo,
        ClaimsPrincipal user,
        int followUserId
    )
    {
        var id = user.UserRealId();
        if (id == null)
        {
            return TypedResults.BadRequest("You don't exist?");
        }
        var follow = await followRepo.GetBy(f =>
            f.FolloweeId == followUserId && f.FollowerId == id
        );
        if (follow == null)
        {
            return TypedResults.BadRequest("You don't follow this user");
        }
        var result = await followRepo.Delete(follow.id);
        return TypedResults.Ok(result);
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<
        Results<Ok<IEnumerable<BlogPost>>, UnauthorizedHttpResult, BadRequest<string>>
    > GetFollowingPosts(
        IRepository<UserFollow> followRepo,
        IRepository<User> userRepo,
        ClaimsPrincipal user
    )
    {
        var id = user.UserRealId();
        if (id == null)
        {
            return TypedResults.BadRequest("You don't exist?");
        }
        var following = (await followRepo.GetAll()).Where(f => f.FollowerId == id);
        var a = following.Select(f => f.Followee).SelectMany(p => p!.Posts!);
        return TypedResults.Ok(a);
    }
}
