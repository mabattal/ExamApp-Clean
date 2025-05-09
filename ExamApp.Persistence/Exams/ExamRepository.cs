using ExamApp.Application.Contracts.Persistence;
using ExamApp.Domain.Entities;
using ExamApp.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Persistence.Exams
{
    public class ExamRepository(AppDbContext context) : GenericRepository<Exam>(context), IExamRepository
    {
        public Task<List<Exam>> GetByInstructorAsync(int instructorId)
        {
            return context.Exams
                .Where(e => !e.IsDeleted && e.CreatedBy == instructorId)
                .Include(e => e.Questions)
                .ToListAsync();
        }

        public Task<List<Exam>> GetActiveExamsAsync()
        {
            return context.Exams
                .Where(e => !e.IsDeleted && e.StartDate <= DateTimeOffset.UtcNow && e.EndDate >= DateTimeOffset.UtcNow && e.Questions.Count > 0)
                .Include(e => e.Instructor)
                .ToListAsync();
        }

        public Task<List<Exam>> GetPastExamsAsync()
        {
            return context.Exams
                .Where(e => !e.IsDeleted && e.EndDate <= DateTimeOffset.UtcNow && e.Questions.Count > 0)
                .Include(e => e.Instructor)
                .ToListAsync();
        }

        public Task<List<Exam>> GetUpcomingExamsAsync()
        {
            return context.Exams
                .Where(e => !e.IsDeleted && e.StartDate >= DateTimeOffset.UtcNow && e.Questions.Count > 0)
                .Include(e => e.Instructor)
                .ToListAsync();
        }

        public Task<Exam?> GetExamWithDetailsAsync(int examId)
        {
            return context.Exams
                .Where(e => !e.IsDeleted)
                .Include(e => e.Questions)
                .Include(e => e.Instructor)
                .FirstOrDefaultAsync(e => e.ExamId == examId);
        }
    }
}
