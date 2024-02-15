using exercise.wwwapi.DataModels;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Controllers
{
    [Route("/api/account")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        public UserController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegistrationDTO regDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appUser = new ApplicationUser
                {
                    UserName = regDTO.Username,
                    Email = regDTO.Email,
                };

                var createUser = await _userManager.CreateAsync(appUser, regDTO.Password!);
                if(createUser.Succeeded)
                {
                    return  Ok(new ResponseDTO
                    {
                        Username = appUser.UserName,
                        Email = appUser.Email,
                        Token = _tokenService.CreateToken(appUser)
                    });
                }
                else
                {
                    return StatusCode(500, "Something bad happened");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }   


    }
}
