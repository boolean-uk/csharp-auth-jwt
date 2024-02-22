using exercise.wwwapi.Data;
using exercise.wwwapi.DTO.Requests;
using exercise.wwwapi.DTO.Response;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Model;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _context;
        private readonly TokenService _tokenService;
        private readonly IRepository _repository;

        public UsersController(UserManager<ApplicationUser> userManager, DataContext context, TokenService tokenService, IRepository repository)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
            _repository = repository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var result = await _userManager.CreateAsync(
                new ApplicationUser { UserName = request.Username, Email = request.Email, Role = request.Role },
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

            if (userInDb is null)
            {
                return Unauthorized();
            }

            var accessToken = _tokenService.CreateToken(userInDb);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponse
            {
                Username = userInDb.UserName,
                Email = userInDb.Email,
                Token = accessToken,
            });
        }


        [HttpPost]
        [Route("helloworld")]
        [Authorize(Roles = "User")]
        public string helloWorld() 
        {
            return "hello world";
        }

        [HttpGet]
        [Route("codes")]
        [Authorize(Roles = "Admin")]       
        public async Task<ActionResult<List<Codes>>> codes()
        {
            return Ok(await _repository.GetCodes());
        }
    }
}
