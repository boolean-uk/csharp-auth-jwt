using System.Security.Claims;
using AutoMapper;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;

namespace exercise.wwwapi.Endpoints;

public static class PostEndpoints
{
    public static void ConfigurePostEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("posts");
        
        group.MapGet("/", GetPosts);
        group.MapGet("/{id}", GetPost);
        group.MapPost("/", CreatePost);
        group.MapPut("/{id}", UpdatePost);
    }

    [Authorize]
    private static async Task<IResult> GetPosts(IRepository<BlogPost> repository, IMapper mapper)
    {
        var posts = await repository.GetAll(p => p.Author);
        var response = new BaseResponse<IEnumerable<BlogPostResponse>>(
            Consts.SuccessStatus,
            mapper.Map<IEnumerable<BlogPostResponse>>(posts)
        );
        
        return TypedResults.Ok(response);
    }
    
    [Authorize]
    private static async Task<IResult> GetPost(IRepository<BlogPost> repository, IMapper mapper, int id)
    {
        var post = await repository.Get(p => p.Id == id, p => p.Author);
        if (post is null)
        {
            return TypedResults.NotFound();
        }
        
        var response = new BaseResponse<BlogPostResponse>(
            Consts.SuccessStatus,
            mapper.Map<BlogPostResponse>(post)
        );
        
        return TypedResults.Ok(response);
    }
    
    [Authorize]
    private static async Task<IResult> CreatePost(IRepository<BlogPost> repository, IMapper mapper, ClaimsPrincipal claims, BlogPostPost body)
    {
        var post = mapper.Map<BlogPost>(body);
        var authorId = claims.Id();
        if (authorId is null)
        {
            return TypedResults.Unauthorized();
        }
        
        post.AuthorId = authorId.Value;
        
        var newPost = await repository.Add(post);
        var response = new BaseResponse<BlogPostResponse>(
            Consts.SuccessStatus,
            mapper.Map<BlogPostResponse>(newPost)
        );
        
        response.Data.Author = claims.DisplayName();
        
        return TypedResults.Created($"/posts/{newPost.Id}", response);
    }
    
    [Authorize]
    private static async Task<IResult> UpdatePost(IRepository<BlogPost> repository, IMapper mapper, ClaimsPrincipal claims, int id, BlogPostPost body)
    {
        var post = await repository.Get(p => p.Id == id, b => b.Author);
        if (post is null)
        {
            return TypedResults.NotFound();
        }
        
        if (post.AuthorId != claims.Id())
        {
            return TypedResults.Unauthorized();
        }
        
        mapper.Map(body, post);
        await repository.Update(post);
        
        var response = new BaseResponse<BlogPostResponse>(
            Consts.SuccessStatus,
            mapper.Map<BlogPostResponse>(post)
        );
        
        return TypedResults.Ok(response);
    }
}