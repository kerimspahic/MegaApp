using API.Data.Dto.Auth;
using API.Models;

namespace API.Data.Mapping
{
    public static class UserMapper
    {
        public static User ToUserFromRegisterDto(this RegisterDto dto)
        {
            return new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}