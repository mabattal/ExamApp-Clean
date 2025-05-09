using System.Reflection;
using ExamApp.Domain.Entities;
using ExamApp.Domain.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ExamApp.Persistence.DbContext
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
    {
        public DbSet<Answer> Answers { get; set; } = null!;
        public DbSet<Exam> Exams { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<ExamResult> ExamResults { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                // appsettings.json dosyasını oku
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Development.json")
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

                // ConnectionStringOption sınıfınızı kullanarak bağlantı dizesini al
                var connectionStrings = configuration.GetSection(ConnectionStringOption.Key)
                    .Get<ConnectionStringOption>();

                optionsBuilder.UseSqlServer(connectionStrings!.SqlServer);

                return new AppDbContext(optionsBuilder.Options);
            }
        }
    }
}
