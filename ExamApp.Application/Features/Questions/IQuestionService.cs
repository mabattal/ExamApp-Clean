using ExamApp.Application.Features.Questions.Create;
using ExamApp.Application.Features.Questions.Dto;
using ExamApp.Application.Features.Questions.Update;

namespace ExamApp.Application.Features.Questions
{
    public interface IQuestionService
    {
        Task<ServiceResult<QuestionResponseDto?>> GetByIdAsync(int id);
        Task<ServiceResult<CreateQuestionResponseDto>> AddAsync(CreateQuestionRequestDto createQuestionRequest, int userId);
        Task<ServiceResult> UpdateAsync(int id, UpdateQuestionRequestDto updateQuestionRequest, int userId);
        Task<ServiceResult> DeleteAsync(int id, int userId);
        Task<ServiceResult<List<QuestionResponseWithoutCorrectAnswerDto>>> GetByExamIdAsync(int examId);
        Task<ServiceResult<List<QuestionResponseDto>>> GetByExamIdWithCorrectAnswerAsync(int examId);

    }
}
