using ExamApp.Application.Contracts.Persistence;
using ExamApp.Domain.Entities;
using ExamApp.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Persistence.Answers
{
    public class AnswerRepository(AppDbContext context) : GenericRepository<Answer>(context), IAnswerRepository
    {
        public Task<List<Answer>> GetByUserAndExamAsync(int userId, int examId)
        {
            return context.Answers
                .Include(a => a.User)
                .Include(a => a.Question)
                .ThenInclude(q => q.Exam)
                .Where(a => a.UserId == userId && a.Question.ExamId == examId)
                .ToListAsync();
        }

        public Task<Answer?> GetByUserAndQuestionAsync(int userId, int questionId)
        {
            return context.Answers
                .Include(a => a.User)
                .Include(a => a.Question)
                .ThenInclude(q => q.Exam)
                .FirstOrDefaultAsync(a => a.UserId == userId && a.QuestionId == questionId);
        }

        public async Task<Answer?> GetByIdWithDetailsAsync(int id)
        {
            return await context.Answers
                .Include(a => a.User)
                .Include(a => a.Question)
                .ThenInclude(q => q.Exam)
                .FirstOrDefaultAsync(a => a.AnswerId == id);
        }
    }
}
