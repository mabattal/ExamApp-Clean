using System.Linq.Expressions;
using ExamApp.Application.Contracts.Persistence;
using ExamApp.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Persistence
{
    public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public Task<List<T>> GetAllAsync()
        {
            return _dbSet.AsNoTracking().ToListAsync();
        }

        public Task<List<T>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            return _dbSet.AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsNoTracking();
        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AnyAsync(predicate);
        }

        public ValueTask<T?> GetByIdAsync(int id) => _dbSet.FindAsync(id);

        public async ValueTask AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);
    }
}
