using ExamApp.Domain.Entities;

namespace ExamApp.Application.Contracts.Persistence
{
    public interface IAnswerRepository : IGenericRepository<Answer>
    {
        Task<List<Answer>> GetByUserAndExamAsync(int userId, int examId);
        Task<Answer?> GetByUserAndQuestionAsync(int userId, int questionId);
        Task<Answer?> GetByIdWithDetailsAsync(int id);
    }
}
