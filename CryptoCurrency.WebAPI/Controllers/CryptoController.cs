using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoCurrency.WebAPI.Controllers
{
    [Route("api/crypto")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        private readonly ICryptoService _cryptoService;

        public CryptoController(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _cryptoService.GetAllAsync());
            }
            catch(Exception ex) {
                return BadRequest(ex);
            }
        }
    }
}
