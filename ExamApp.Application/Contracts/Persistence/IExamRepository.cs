using ExamApp.Domain.Entities;

namespace ExamApp.Application.Contracts.Persistence
{
    public interface IExamRepository : IGenericRepository<Exam>
    {
        Task<List<Exam>> GetByInstructorAsync(int instructorId);
        Task<List<Exam>> GetActiveExamsAsync();
        Task<List<Exam>> GetPastExamsAsync();
        Task<List<Exam>> GetUpcomingExamsAsync();
        Task<Exam?> GetExamWithDetailsAsync(int examId);    //Sınava ait detayları (sorular ve eğitmen bilgisi ile birlikte) getirme.
    }
}
