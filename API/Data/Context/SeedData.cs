using API.Interfaces;
using API.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Context
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var context = services.GetRequiredService<AppDbContext>();
            var passwordHasher = services.GetRequiredService<IPasswordHasherService>();
            
            await context.Database.EnsureCreatedAsync();

            if (!context.Role.Any())
            {
                context.Role.AddRange(
                    new Role { RoleName = "User", Description = "Regular user role" },
                    new Role { RoleName = "Manager", Description = "Manager role" },
                    new Role { RoleName = "Admin", Description = "Administrator role" }
                );
                await context.SaveChangesAsync();
            }

            var adminUser = await context.Users.FirstOrDefaultAsync(user => user.UserName == "admin");

            if (adminUser == null)
            {
                var passwordHash = Argon2.Hash("Admin@123");
                adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@mail.com",
                    PasswordHash = passwordHasher.HashPassword("Admin@123"),
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }

            var adminRole = await context.Role.FirstOrDefaultAsync(r => r.RoleName == "Admin");
            if (adminRole != null && !context.UserRoles.Any(ur => ur.UserId == adminUser.UserId && ur.RoleId == adminRole.RoleId))
            {
                var userRole = new UserRole
                {
                    UserId = adminUser.UserId,
                    RoleId = adminRole.RoleId,
                    AssignedAt = DateTime.UtcNow
                };

                context.UserRoles.Add(userRole);
                await context.SaveChangesAsync();
            }
        }
    }
}