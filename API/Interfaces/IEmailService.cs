namespace API.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string reciver, string subject, string body);
    }
}