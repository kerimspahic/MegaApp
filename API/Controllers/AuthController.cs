using API.Data.Dto.Auth;
using API.Data.Dto.User;
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
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return BadRequest("Password and Confirm Password do not match.");
            }

            var result = await _authService.Register(registerDto);
            if (!result)
            {
                return StatusCode(500, "An error occurred while creating the user.");
            }

            return Ok("User registered successfully.");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.Login(loginDto);

            if (result == null)
                return Unauthorized("Invalid credentials or account locked.");
            else if (result == "Account is locked. Try again later.")
                return Forbid(result);
            return Ok(result);
        }
    }
}