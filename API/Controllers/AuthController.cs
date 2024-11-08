using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthService _authservice;

        public AuthController(AuthService authService)
        {
            _authservice = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(string userName, string password, string email)
        {
            try
            {
                await _authservice.Register(userName, password, email);
                return Ok();
            }
            catch (ArgumentException ex)
            {
            return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            try
            {
                await _authservice.Login(userName,password);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}