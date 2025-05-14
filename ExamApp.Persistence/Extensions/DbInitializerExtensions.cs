using ExamApp.Application.Contracts.Authentication;
using ExamApp.Persistence.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExamApp.Persistence.Extensions
{
    public static class DbInitializerExtensions
    {
        public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
                AppDbContextSeed.SeedAsync(dbContext, passwordHasher).Wait();
            }
            return app;
        }
    }
}
