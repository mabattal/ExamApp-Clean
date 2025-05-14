using ExamApp.Application.Contracts.Authentication;
using ExamApp.Domain.Entities;
using ExamApp.Domain.Enums;

namespace ExamApp.Persistence.DbContext
{
    public static class AppDbContextSeed
    {
        public static async Task SeedAsync(AppDbContext context, IPasswordHasher passwordHasher)
        {
            // Eğer hiç admin yoksa ekle
            if (!context.Users.Any(u => u.Role == UserRole.Admin))
            {
                var adminUser = new User
                {
                    FullName = "Admin",
                    Email = "admin@admin.com",
                    Password = passwordHasher.Hash("Admin123"),
                    Role = UserRole.Admin,
                    IsDeleted = false
                };

                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}
