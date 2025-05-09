using ExamApp.Domain.Entities;
using ExamApp.Domain.Enums;

namespace ExamApp.Application.Contracts.Persistence
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetInstructorByIdAsync(int userId, UserRole userRole);
        Task<List<User>> GetByRoleAsync(UserRole userRole);
    }
}
