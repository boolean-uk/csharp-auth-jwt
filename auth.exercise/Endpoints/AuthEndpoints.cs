using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using auth.exercise.Model;
using auth.exercise.DTO;
using auth.exercise.Services;
using auth.exercise.Enums;
using auth.exercise.Repository;

namespace auth.exercise.Endpoints
{
  public static class AuthEndpoints
  {
    public static void ConfigureAuthEndpoints(this WebApplication app)
    {
      var taskGroup = app.MapGroup("auth");
      taskGroup.MapPost("/register", Register);
      taskGroup.MapPost("/login", Login);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public static async Task<IResult> Register(
      RegisterDto registerPayload,
      UserManager<ApplicationUser> userManager)
    {
      if (registerPayload.Email == null) return TypedResults.BadRequest("Email is required.");
      if (registerPayload.Password == null) return TypedResults.BadRequest("Password is required.");

      var result = await userManager.CreateAsync(
          new ApplicationUser { UserName = registerPayload.Email, Email = registerPayload.Email, Role = Roles.User },
        registerPayload.Password!
      );

      if (result.Succeeded)
      {
        return TypedResults.Created($"/auth/", new { email = registerPayload.Email, role = Roles.User });
      }
      return Results.BadRequest(result.Errors);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public static async Task<IResult> Login(
      LoginDto loginPayload,
      UserManager<ApplicationUser> userManager,
      TokenService tokenService,
      IProductRepository repository)
    {
      if (loginPayload.Email == null) return TypedResults.BadRequest("Email is required.");
      if (loginPayload.Password == null) return TypedResults.BadRequest("Password is required.");

      var user = await userManager.FindByEmailAsync(loginPayload.Email!);
      if (user == null)
      {
        return TypedResults.BadRequest("Invalid email or password.");
      }

      var isPasswordValid = await userManager.CheckPasswordAsync(user, loginPayload.Password);
      if (!isPasswordValid)
      {
        return TypedResults.BadRequest("Invalid email or password.");
      }

      var userInDb = repository.GetUser(loginPayload.Email);

      if (userInDb is null)
      {
        return Results.Unauthorized();
      }

      var accessToken = tokenService.CreateToken(userInDb);
      return TypedResults.Ok(new AuthResponseDto(accessToken, userInDb.Email, userInDb.Role));
    }

  }
}
