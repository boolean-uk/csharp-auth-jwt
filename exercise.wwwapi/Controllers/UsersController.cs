using exercise.wwwapi.Data;
using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransfer.Request;
using exercise.wwwapi.DataTransfer.Response;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Services;
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

        public UsersController(UserManager<ApplicationUser> userManager, DataContext context, TokenService tokenService, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _dataContext = context;
            _tokenService = tokenService;
        }



        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser { UserName = request.Username, Email = request.Email, Role = request.role };
               // request.Password!
            

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                
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
                return BadRequest("Bad email");
            }

            var isPassordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);

            if (!isPassordValid)
            {
                return BadRequest("Bad password");
            }

            var userInDb = _dataContext.Users.FirstOrDefault(u => u.Email == request.Email);

            if (userInDb == null)
            {
                return Unauthorized();
            }

            var AccessToken = _tokenService.CreateToken(userInDb);
            await _dataContext.SaveChangesAsync();

            return Ok(new AuthResponse
            {
                Username = userInDb.UserName,
                Email = userInDb.Email,
                Token = AccessToken
            });
        }
    }
}
