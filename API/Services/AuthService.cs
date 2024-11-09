using API.Data.Context;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        private const int MaxFailedAttempts = 5;
        private readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

        public AuthService(AppDbContext context, IPasswordHasherService passwordHasher, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<bool> Register(string userName, string password, string email)
        {
            string roleName = "User";

            var role = await _context.Role.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
            {
                throw new ArgumentException("Role does not exist.");
            }

            var passwordHash = _passwordHasher.HashPassword(password);

            var user = new User
            {
                UserName = userName,
                PasswordHash = passwordHash,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userRole = new UserRole
            {
                UserId = user.UserId,
                RoleId = role.RoleId,
                AssignedAt = DateTime.UtcNow
            };
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<string> Login(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
                return null;

            if (user.LockoutEnabled && user.LockoutEnd > DateTime.UtcNow)
                return "Account is locked. Try again in 15 min";

            if (_passwordHasher.VerifyPassword(user.PasswordHash, password))
            {
                user.FailedLoginAttempts = 0;
                user.LockoutEnabled = false;
                user.LockoutEnd = null;

                await _context.SaveChangesAsync();

                var roles = await _context.UserRoles
                    .Where(ur => ur.UserId == user.UserId)
                    .Select(ur => ur.Role.RoleName)
                    .ToListAsync();

                return _jwtTokenService.GenerateToken(user, roles);
            }
            else
            {
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= MaxFailedAttempts)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTime.UtcNow.Add(LockoutDuration);
                }

                await _context.SaveChangesAsync();

                return null;
            }
        }
    }
}