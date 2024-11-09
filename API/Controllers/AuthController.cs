using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(string userName, string password, string email)
        {
            try
            {
                await _authService.Register(userName, password, email);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var result = await _authService.Login(userName, password);

            if (result == null)
                return Unauthorized("Invalid credentials or account locked.");
            else if (result == "Account is locked. Try again later.")
                return Forbid(result);
            return Ok(new { Token = result });
        }
    }
}