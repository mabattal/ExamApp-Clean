using ExamApp.Application.Features.Questions.Dto;
using ExamApp.Application.Features.Users.Dto;

namespace ExamApp.Application.Features.Exams.Dto
{
    public record ExamWithDetailsResponseDto(
        int ExamId,
        string Title,
        string Description,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate,
        int Duration,
        UserResponseDto Instructor,
        ICollection<QuestionResponseDto> Questions
    );
}