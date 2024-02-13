using csharp_auth_jwt.Model;
using csharp_auth_jwt.Model.Dto;
using csharp_auth_jwt.Model.Enum;
using Microsoft.AspNetCore.Identity;

namespace csharp_auth_jwt.Endpoints
{
    public static class AuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("auth");
            authGroup.MapPost("/register" , Register);
            authGroup.MapPost("/login" , Login);
        }

        public static async Task<IResult> Register(
          RegisterDto registerPayload ,
          UserManager<BlogUser> userManager ,
          TokenService tokenService)
        {
            var user = new BlogUser { UserName = registerPayload.Email , Email = registerPayload.Email , Role = BlogUserRole.User };
            var result = await userManager.CreateAsync(user , registerPayload.Password);

            if(result.Succeeded)
            {
                return Results.Created($"/auth/{user.Id}" , new { Token = tokenService.CreateToken(user) });
            }
            return Results.BadRequest(result.Errors);
        }

        public static async Task<IResult> Login(
          LoginDto loginPayload ,
          UserManager<BlogUser> userManager ,
          TokenService tokenService)
        {
            var user = await userManager.FindByEmailAsync(loginPayload.Email);
            if(user != null && await userManager.CheckPasswordAsync(user , loginPayload.Password))
            {
                return Results.Ok(new { Token = tokenService.CreateToken(user) });
            }
            return Results.BadRequest("Invalid email or password.");
        }
    }
}
