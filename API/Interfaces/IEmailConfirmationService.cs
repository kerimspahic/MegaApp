namespace API.Interfaces
{
    public interface IEmailConfirmationService
    {
        Task<bool> SendConfirmationEmailAsync(int userId, string confirmationUrlBase);
        Task<bool> ConfirmEmailAsync(int userId, string token);
    }
}
