using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoCurrency.WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserData()
        {
            try
            {
                return Ok(await _userService.GetUserData());
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
