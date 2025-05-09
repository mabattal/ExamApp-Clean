using ExamApp.Domain.Entities;

namespace ExamApp.Application.Contracts.Persistence
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<List<Question>> GetByExamIdAsync(int examId);
        Task<Question?> GetByIdWithExamAsync(int questionId);
    }
}
