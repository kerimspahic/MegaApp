using API.Data.Dto.Auth;
using API.Data.Dto.User;

namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterDto registerDto);
        Task<string> Login(LoginDto loginDto);
    }
}