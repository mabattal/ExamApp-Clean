using ExamApp.Application.Contracts.Persistence;
using ExamApp.Domain.Entities;
using ExamApp.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Persistence.ExamResults
{
    public class ExamResultRepository(AppDbContext context) : GenericRepository<ExamResult>(context), IExamResultRepository
    {
        public Task<decimal> GetAverageScoreByExamAsync(int examId)
        {
            return context.ExamResults
                .Where(er => er.ExamId == examId && er.Score != null)
                .AverageAsync(er => (decimal)er.Score!);
        }

        public Task<decimal> GetMaxScoreByExamAsync(int examId)
        {
            return context.ExamResults
                .Where(er => er.ExamId == examId && er.Score != null)
                .MaxAsync(er => (decimal)er.Score!);
        }

        public Task<decimal> GetMinScoreByExamAsync(int examId)
        {
            return context.ExamResults
                .Where(er => er.ExamId == examId && er.Score != null)
                .MinAsync(er => (decimal)er.Score!);
        }

        public Task<int> GetExamCountByExamAsync(int examId)
        {
            return context.ExamResults
                .Where(er => er.ExamId == examId)
                .CountAsync();
        }

        public Task<List<ExamResult>> GetByUserIdAsync(int userId)
        {
            return context.ExamResults
                .Where(er => er.UserId == userId && er.CompletionDate != null)
                .Include(er => er.Exam)
                .Include(er => er.User)
                .ToListAsync();
        }

        public Task<ExamResult?> GetByUserIdAndExamIdAsync(int userId, int examId)
        {
            return context.ExamResults
                .Include(er => er.Exam)
                .Include(er => er.User)
                .FirstOrDefaultAsync(er => er.UserId == userId && er.ExamId == examId);
        }

        public Task<List<ExamResult>> GetByExamIdAsync(int examId)
        {
            return context.ExamResults
                .Include(er => er.User)
                .Where(er => er.ExamId == examId)
                .ToListAsync();
        }

        public Task<List<ExamResult>> GetExpiredResultsAsync()
        {
            return context.ExamResults
                .Where(x => x.CompletionDate == null &&
                            (x.StartDate.AddMinutes(x.Exam.Duration) <= DateTimeOffset.UtcNow ||
                             (x.StartDate.AddMinutes(x.Exam.Duration) > x.Exam.EndDate &&
                              x.Exam.EndDate <= DateTimeOffset.UtcNow)))
                .Include(x => x.Exam)
                .ToListAsync();
        }
    }
}
