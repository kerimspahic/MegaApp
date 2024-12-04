using System.Net;
using System.Net.Mail;
using API.Data.Configuration;
using API.Interfaces;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSetings _smtpSetings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SmtpSetings> smtpSetings, ILogger<EmailService> logger)
        {
            _smtpSetings = smtpSetings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string reciver, string subject, string body)
        {
            try
            {
                var client = new SmtpClient(_smtpSetings.Server, _smtpSetings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSetings.Username, _smtpSetings.Password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSetings.Sender),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(reciver);

                await client.SendMailAsync(mailMessage);
                System.Console.WriteLine("Sent");

                _logger.LogInformation($"Email sent successfully to {reciver}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while sending email: {ex.Message}");
                throw;
            }
        }
    }
}