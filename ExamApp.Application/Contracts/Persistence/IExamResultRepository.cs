using ExamApp.Domain.Entities;

namespace ExamApp.Application.Contracts.Persistence
{
    public interface IExamResultRepository : IGenericRepository<ExamResult>
    {
        Task<decimal> GetAverageScoreByExamAsync(int examId);
        Task<decimal> GetMaxScoreByExamAsync(int examId);
        Task<decimal> GetMinScoreByExamAsync(int examId);
        Task<int> GetExamCountByExamAsync(int examId);
        Task<List<ExamResult>> GetByUserIdAsync(int userId);
        Task<ExamResult?> GetByUserIdAndExamIdAsync(int userId, int examId);
        Task<List<ExamResult>> GetByExamIdAsync(int examId);
        Task<List<ExamResult>> GetExpiredResultsAsync();
    }

}
