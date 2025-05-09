using ExamApp.Application.Contracts.Persistence;
using ExamApp.Persistence.DbContext;

namespace ExamApp.Persistence
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public Task<int> SaveChangeAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
