using API.Data.Context;
using API.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Register(string userName, string password, string email)
        {
            var passwordHash = Argon2.Hash(password);

            var user = new User
            {
                UserName = userName,
                PasswordHash = passwordHash,
                Email = email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Login(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null) 
                return false;

            if (Argon2.Verify(user.PasswordHash, password))
                return true;

            return false;
        }
    }
}