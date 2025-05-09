using ExamApp.Application.Contracts.Persistence;
using ExamApp.Domain.Entities;
using ExamApp.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Persistence.Questions
{
    public class QuestionRepository(AppDbContext context) : GenericRepository<Question>(context), IQuestionRepository
    {
        public Task<List<Question>> GetByExamIdAsync(int examId)
        {
            return context.Questions
                .Where(q => q.ExamId == examId)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<Question?> GetByIdWithExamAsync(int questionId)
        {
            return context.Questions
                .Where(q => q.QuestionId == questionId)
                .Include(q => q.Exam)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}