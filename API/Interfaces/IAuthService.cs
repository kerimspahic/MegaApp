namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(string userName, string password, string email);
        Task<bool> Login(string userName, string password);
    }
}