using ExamApp.Application.Contracts.Persistence;
using ExamApp.Domain.Entities;
using ExamApp.Domain.Enums;
using ExamApp.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Persistence.Users
{
    public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
    {
        public Task<User?> GetByEmailAsync(string email)
        {
            return context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<User?> GetInstructorByIdAsync(int userId, UserRole userRole)
        {
            return context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId && u.Role == userRole);
        }

        public Task<List<User>> GetByRoleAsync(UserRole userRole)
        {
            return context.Users.AsNoTracking()
                .Where(u => u.Role == userRole)
                .ToListAsync();
        }
    }
}
