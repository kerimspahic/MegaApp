using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    public class EmailConfirmationController : Controller
    {
        private readonly IEmailConfirmationService _emailConfirmationService;

        public EmailConfirmationController(IEmailConfirmationService emailConfirmationService)
        {
            _emailConfirmationService = emailConfirmationService;
        }

        [HttpPost("SendConfirmationEmail")]
        public async Task<IActionResult> SendConfirmationEmail(int userId)
        {
            var confirmationUrlBase = Url.Action(
                nameof(ConfirmEmail),
                "EmailConfirmation",
                null,
                Request.Scheme
            );

            var result = await _emailConfirmationService.SendConfirmationEmailAsync(userId, confirmationUrlBase);

            if (!result)
                return BadRequest("User not found or email is already confirmed.");

            return Ok("Confirmation email sent successfully.");
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            var result = await _emailConfirmationService.ConfirmEmailAsync(userId, token);

            if (!result)
                return BadRequest("Invalid user ID or token.");

            return Ok("Email confirmed successfully.");
        }
    }
}
