using CryptoCurrency.Model.DTO.Auth;
using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoCurrency.WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            try
            {
                var result = await _userService.RegisterUserAsync(model);

                if (result.Succeeded)
                    return Created();
                return Conflict(new { message = "User with this credentials already exist" });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred. Please try again later." });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var token = await _userService.LoginUserAsync(model);
                return Ok(token);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred. Please try again later." });
            }
        }
    }
}
