namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(string userName, string password, string email);
        Task<string> Login(string userName, string password);
    }
}