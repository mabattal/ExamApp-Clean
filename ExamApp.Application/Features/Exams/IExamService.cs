using ExamApp.Application.Features.Exams.Create;
using ExamApp.Application.Features.Exams.Dto;
using ExamApp.Application.Features.Exams.Update;

namespace ExamApp.Application.Features.Exams
{
    public interface IExamService
    {
        Task<ServiceResult<CreateExamResponseDto>> AddAsync(CreateExamRequestDto examRequest, int userId);
        Task<ServiceResult> UpdateAsync(int id, UpdateExamRequestDto examRequest, int userId);
        Task<ServiceResult> DeleteAsync(int id, int userId);
        Task<ServiceResult<List<ExamWithQuestionsResponseDto>>> GetByInstructorAsync(int instructorId);          // Eğitmene ait sınavları listelemek için
        Task<ServiceResult<List<ExamWithInstructorResponseDto>>> GetActiveExamsAsync();                           // Şu an aktif olan sınavları getirmek için
        Task<ServiceResult<List<ExamWithInstructorResponseDto>>> GetPastExamsAsync();
        Task<ServiceResult<List<ExamWithInstructorResponseDto>>> GetUpcomingExamsAsync();
        Task<ServiceResult<ExamWithDetailsResponseDto?>> GetByIdAsync(int id);                                 // Id'ye göre sınav getirmek için
    }
}
