using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace exercise.wwwapi.EndPoints;

public static class AuthApi
{
    public static void ConfigureAuthApi(this WebApplication app)
    {
        app.MapPost("register", Register);
        app.MapPost("login", Login);
        app.MapGet("users", GetUsers).RequireAuthorization();
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private static async Task<IResult> GetUsers(IRepository<User> repo, ClaimsPrincipal user)
    {
        return TypedResults.Ok(await repo.GetAll());
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    private static async Task<IResult> Register(UserRequestDto request, IRepository<User> repo)
    {
        //user exists
        if (await UserExists(repo, request.Username))
            return Results.Conflict(
                new Payload<UserRequestDto>()
                {
                    status = "Username already exists!",
                    data = request,
                }
            );

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User()
        {
            Username = request.Username,
            PasswordHash = passwordHash,
            Email = request.Email,
        };

        await repo.Insert(user);

        return Results.Ok(new Payload<string>() { data = "Created Account" });
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> Login(
        UserRequestDto request,
        IRepository<User> repo,
        IConfigurationSettings config
    )
    {
        User? user = await GetByUsername(repo, request.Username);
        if (user == null)
            return Results.BadRequest(
                new Payload<UserRequestDto>() { status = "User does not exist", data = request }
            );

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Results.BadRequest(
                new Payload<UserRequestDto>() { status = "Wrong Password", data = request }
            );
        }
        string token = CreateToken(user, config);
        return Results.Ok(new Payload<string>() { data = token });
    }

    private static string CreateToken(User user, IConfigurationSettings config)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config.GetValue("AppSettings:Token"))
        );
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    private static async Task<bool> UserExists(IRepository<User> repo, string username)
    {
        return (await repo.GetBy(user => user.Username == username)) != null;
    }

    private static async Task<User?> GetByUsername(IRepository<User> repo, string username)
    {
        return await repo.GetBy(user => user.Username == username);
    }
}
