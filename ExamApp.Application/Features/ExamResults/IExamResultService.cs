using ExamApp.Application.Features.ExamResults.Dto;

namespace ExamApp.Application.Features.ExamResults
{
    public interface IExamResultService
    {
        Task<ServiceResult<ExamResultResponseDto?>> GetByIdAsync(int id);
        Task<ServiceResult<ExamResultResponseDto?>> GetByUserIdAndExamId(int userId, int examId);
        Task<ServiceResult<ExamResultResponseDto?>> GetByUserAndExam(int userId, int examId);
        Task<ServiceResult> StartExamAsync(int examId, int userId);
        Task<ServiceResult> SubmitExamAsync(int examId, int userId);
        Task<ServiceResult<List<ExamResultResponseDto>>> GetByUserIdAsync(int userId);
        Task<ServiceResult<ExamResultStatisticsResponseDto>> GetStatisticsByExamAsync(int examId);
        Task<ServiceResult> AutoSubmitExpiredExamsAsync();                                          // Sınavı otomatik tamamlamayı sağlayan metot
        Task<ServiceResult<List<ExamResultResponseDto>>> GetByExamIdAsync(int examId);
    }
}
