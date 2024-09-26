using AutoMapper;
using exercise.Application;
using exercise.Data.Enums;
using exercise.Data.Models;
using exercise.Infrastructure;
using exercise.wwwapi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace exercise.wwwapi.Endpoints
{
    public static class UserEndpoint
    {
        public static void ConfigureUserEndpoint(this WebApplication app)
        {
            var group = app.MapGroup("/users");
            group.MapGet("/{id}", Get);
            group.MapGet("/", GetAll);
        }

        [Authorize(Roles = "Admin")]
        public static async Task<IResult> Get(UserService userService, string id)
        {
            var response = await userService.Get(id);
            if (!response.Success)
            {
                return TypedResults.NotFound(response); 
            }
            return TypedResults.Ok(response);
        }

        [Authorize(Roles = "Admin")]
        public static async Task<IResult> GetAll(UserService userService)
        {
            var response = await userService.GetAll();
            if (!response.Success)
            {
                return TypedResults.NotFound(response);
            }
            return TypedResults.Ok(response);
        }
    }
}
