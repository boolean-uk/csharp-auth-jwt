using exercise.wwwapi.Data;
using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransfers.Requests;
using exercise.wwwapi.DataTransfers.Responses;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Controllers
{
    [ApiController]
    [Route("/api[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _context;
        private readonly TokenService _tokenService;

        public UserController(UserManager<ApplicationUser> userManager, DataContext context, TokenService tokenService)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(Registration request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManager.CreateAsync(new ApplicationUser {
                UserName = request.Username, Email = request.Email, Role = request.Role },
                request.Password!
                );

            if (result.Succeeded)
            {
                request.Password = "";
                return CreatedAtAction(nameof(Register), new { email = request.Email, role = Role.User }, request);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var managedUser = await _userManager.FindByEmailAsync(request.Email!);

            if (managedUser == null)
            {
                return BadRequest("Bad credentials");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password!);

            if (!isPasswordValid)
            {
                return BadRequest("Bad credentials");
            }

            var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (userInDb == null)
            {
                return Unauthorized();
            }

            var accessToken = _tokenService.CreateToken(userInDb);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponse
            {
                Username = userInDb.UserName,
                Email = userInDb.Email,
                Token = accessToken
            });
        }
    }
}
