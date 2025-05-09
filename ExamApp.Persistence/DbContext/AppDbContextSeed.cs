using ExamApp.Application.Authentication;
using ExamApp.Domain.Entities;
using ExamApp.Domain.Enums;

namespace ExamApp.Persistence.DbContext
{
    public static class AppDbContextSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Eğer en az bir admin yoksa ekle
            if (!context.Users.Any(u => u.Role == UserRole.Admin))
            {
                var adminUser = new User
                {
                    FullName = "Admin",
                    Email = "admin@admin.com",
                    Password = PasswordHasher.Hash("Admin123"),
                    Role = UserRole.Admin,
                    IsDeleted = false
                };

                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }


}
