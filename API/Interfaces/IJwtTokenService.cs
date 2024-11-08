using API.Models;

namespace API.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user, List<string> roles);
    }
}