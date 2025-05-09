using ExamApp.Domain.Entities;

namespace ExamApp.Application.Features.ExamResults.Dto
{
    public record ExamResultResponseDto(
        int ResultId,
        int UserId,
        User User,
        int ExamId,
        decimal? Score,
        DateTimeOffset StartDate,
        DateTimeOffset? CompletionDate,
        int? Duration,
        int TotalQuestions,
        int? CorrectAnswers,
        int? IncorrectAnswers,
        int? EmptyAnswers
    );
}
