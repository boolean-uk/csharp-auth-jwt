using exercise.wwwapi.Data;
using exercise.wwwapi.DataTransfer.Request;
using exercise.wwwapi.DataTransfer.Response;
using exercise.wwwapi.Models;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _dataContext;
        private readonly TokenService _tokenService;
       
        public UsersController(UserManager<ApplicationUser> userManager, DataContext dataContext, TokenService tokenService)
        {
            _userManager = userManager;
            _dataContext = dataContext;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegistrationRequest registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userManager.CreateAsync(
                new ApplicationUser { UserName = registration.UserName, Email = registration.Email, Role = registration.Role },
                registration.Password!
            );

            if (result.Succeeded)
            {
                registration.Password = "";
                return CreatedAtAction(nameof(Register), new { email = registration.Email, Role = registration.Role }, registration );
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByEmailAsync(request.Email!);
            if (user == null)
            {
                return BadRequest("No account with that email");
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password!);
            if (!isPasswordValid) 
            {
                return BadRequest("Wrong password");
            }
            var userInDb = _dataContext.Users.FirstOrDefault(u => u.Email == request.Email);
            if (userInDb == null)
            {
                return Unauthorized();
            }
            var accessToken = _tokenService.CreateToken(userInDb);
            await _dataContext.SaveChangesAsync();
            return Ok(new AuthResponse
            {
                UserName = userInDb.UserName,
                Email = userInDb.Email,
                Token = accessToken
            });
        }

    }
}
