using API.Data.Context;
using API.Interfaces;
using API.Models;

namespace API.Services
{
    public class EmailConfirmationService : IEmailConfirmationService
    {
        private readonly IEmailService _emailService;
        private readonly AppDbContext _dbContext;

        public EmailConfirmationService(IEmailService emailService, AppDbContext dbContext)
        {
            _emailService = emailService;
            _dbContext = dbContext;
        }

        public async Task<bool> SendConfirmationEmailAsync(int userId, string confirmationUrlBase)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null || user.EmailConfirmed)
                return false;

            var confirmationLink = GenerateConfirmationLink(userId, user.Email, confirmationUrlBase);
            var subject = "Email Confirmation";
            var body = $"<p>Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.</p>";

            await _emailService.SendEmailAsync(user.Email, subject, body);
            return true;
        }

        public async Task<bool> ConfirmEmailAsync(int userId, string token)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null || user.EmailConfirmed)
                return false;

            if (!ValidateToken(user, token))
                return false;

            user.EmailConfirmed = true;
            user.UpdatedAt = DateTime.UtcNow;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        private string GenerateConfirmationLink(int userId, string email, string baseUrl)
        {
            var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{userId}:{email}:{DateTime.UtcNow}"));
            return $"{baseUrl}?userId={userId}&token={token}";
        }

        private bool ValidateToken(User user, string token)
        {
            try
            {
                var tokenData = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split(':');
                return tokenData[0] == user.UserId.ToString() && tokenData[1] == user.Email;
            }
            catch
            {
                return false;
            }
        }
    }
}
