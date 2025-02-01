
using System.Security.Claims;
using api_cinema_challenge.DTO;
using api_cinema_challenge.DTO.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTO.Request;
using exercise.wwwapi.DTO.Response;
using exercise.wwwapi.Models;
using exercise.wwwapi.Payload;
using Microsoft.AspNetCore.Http.HttpResults;

namespace exercise.wwwapi.Endpoints
{
    public static class AuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            var usergroup = app.MapGroup("auth");
            usergroup.MapPost("/register", CreateUser);
            usergroup.MapPost("/login", LoginUser);
        }

        private static async Task<IResult> LoginUser(HttpContext context, IRepository<User> repo,  IConfigurationSettings conf,Auth_Login_User dto)
        {
            try
            {
                await Auth_Login_User.authenticate(dto, repo, conf);
                return TypedResults.Ok(Auth_Login_User.toPayloadAuth(dto));
            }
            catch (HttpRequestException ex)
            {
                return Fail.Payload(ex);
            }
        }

        private static async Task<IResult> CreateUser(HttpContext context, IRepository<User> repo, ClaimsPrincipal user,Create_User dto)
        {
            if (await repo.GetEntry(x => x.Where(x => x.Username == dto.Username)) != null) return Fail.Payload("user already existed with that name", TypedResults.Conflict);
            try
            {
                User createduser = await dto.Create(repo, user);
                return TypedResults.Ok(Get_User.toPayload(createduser));
            }
            catch (HttpRequestException ex)
            {
                return Fail.Payload(ex);
            }
        }
    }
}
