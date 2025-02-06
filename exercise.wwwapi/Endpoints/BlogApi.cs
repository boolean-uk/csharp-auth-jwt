using System;
using Microsoft.AspNetCore.Authorization;

namespace exercise.wwwapi.Endpoints;

public static class BlogApi
{
    public static void ConfigureBlogApi(this WebApplication app)
    {
        var blog = app.MapGroup("/posts");

        blog.MapGet("/", GetAllBlogPosts);
        blog.MapPost("/", CreateNewPost);
        blog.MapPut("/", UpdatePost);
    }

    [Authorize]
    private static async Task UpdatePost(HttpContext context)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    private static async Task CreateNewPost(HttpContext context)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    private static async Task GetAllBlogPosts(HttpContext context)
    {
        throw new NotImplementedException();
    }
}
