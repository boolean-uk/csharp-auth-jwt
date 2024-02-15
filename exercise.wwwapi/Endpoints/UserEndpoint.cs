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
        public static async Task<IResult> Get(IRepository<User> repository, IMapper mapper, string id)
        {
            ServiceResponse<GetUserDTO> response = new();
            try
            {
                User user = await repository.Get(id);
                response.Data = mapper.Map<GetUserDTO>(user);
                return TypedResults.Ok(response);
            } catch (ArgumentException ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return TypedResults.NotFound(response);
            }
        }

        [Authorize(Roles = "Admin")]
        public static async Task<IResult> GetAll(IRepository<User> repository, IMapper mapper)
        {
            ServiceResponse<List<GetUserDTO>> response = new();
            try
            {
                List<User> users = await repository.GetAll();
                response.Data = users.Select(mapper.Map<GetUserDTO>).ToList();
                return TypedResults.Ok(response);
            } catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return TypedResults.BadRequest(response);
            }
        }
    }
}
