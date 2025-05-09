using ExamApp.Application.Features.Questions.Dto;

namespace ExamApp.Application.Features.Exams.Dto
{
    public record ExamWithQuestionsResponseDto(
        int ExamId,
        string Title,
        string Description,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate,
        int Duration,
        ICollection<QuestionResponseDto> Questions
    );
}
