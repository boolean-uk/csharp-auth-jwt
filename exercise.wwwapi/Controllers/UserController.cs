using exercise.wwwapi.DataModels;
using exercise.wwwapi.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Controllers
{
    [Route("/api/account")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
                    return CreatedAtAction(nameof(Register), regDTO);
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
