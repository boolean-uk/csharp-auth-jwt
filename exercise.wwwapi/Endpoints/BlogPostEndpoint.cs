using exercise.wwwapi.Repository;
using exercise.wwwapi.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using exercise.wwwapi.DataTransfer.Request;
using System.Security.Claims;
using exercise.wwwapi.Helpers;

namespace exercise.wwwapi.Endpoints;

public static class BlogPostEndpoint
{
    public static void ConfigureBlogPostEndpoint(this WebApplication app)
    {

        var blogposts = app.MapGroup("blogposts");
        blogposts.MapPost("/", CreateBlogPost);
        blogposts.MapGet("/", GetAllBlogPosts);
        blogposts.MapGet("/{id}", GetBlogPost);
        blogposts.MapPut("/{id}", UpdateBlogPost);
        blogposts.MapDelete("/{id}", DeleteBlogPost);
    }

    [Authorize(Roles = "User, Admin")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> CreateBlogPost(IRepository<BlogPost> repository, IRepository<User> userRepository, ClaimsPrincipal userData, BlogPostPostRequest postRequest)
    {
        //userData.UserId()
        var user = await userRepository.GetById(userData.UserId());
        if (user == null)
        {
            return TypedResults.NotFound($"User with the Id: {userData.UserId()} can be found!");
        }
        var newPost = new BlogPost()
        {
            Text = postRequest.Text,
            UserId = userData.UserId(),
        };
        var result = await repository.Add(newPost);
        return TypedResults.Created($"/{result.Id}", new { BlogPost = BlogPostDTO.ToDTO(result) });
    }

    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> GetAllBlogPosts(IRepository<BlogPost> repository)
    {
        var posts = await repository.GetAll();
        var returnList = new List<BlogPostDTO>();
        foreach (var post in posts)
        {
            returnList.Add(BlogPostDTO.ToDTO(post));
        }
        return TypedResults.Ok(new { BlogPosts=returnList } );
    }

    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> GetBlogPost(IRepository<BlogPost> repository, int id)
    {
        var post = await repository.GetById(id);
        if (post == null)
        {
            return TypedResults.NotFound($"Post with the Id: {id} can be found!");
        }
        return TypedResults.Ok(new { BlogPost = BlogPostDTO.ToDTO(post) });
    }

    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> UpdateBlogPost(IRepository<BlogPost> repository, int id, BlogPostPutRequest putRequest)
    {
        var post = await repository.GetById(id);
        if (post == null)
        {
            return TypedResults.NotFound($"Post with the Id: {id} can be found!");
        }
        post.Text = string.IsNullOrEmpty(putRequest.Text) ? post.Text : putRequest.Text;
        var result = await repository.Update(post);
        return TypedResults.Ok(new { BlogPost = BlogPostDTO.ToDTO(result) });
    }

    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public static async Task<IResult> DeleteBlogPost(IRepository<BlogPost> repository, int id)
    {
        var post = await repository.GetById(id);
        if (post == null)
        {
            return TypedResults.NotFound($"Post with the Id: {id} can be found!");
        }
        var result = await repository.Delete(post);
        return TypedResults.Ok(new { BlogPost = BlogPostDTO.ToDTO(result) });
    }
}
