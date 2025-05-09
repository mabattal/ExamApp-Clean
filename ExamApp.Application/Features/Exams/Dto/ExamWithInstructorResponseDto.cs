using ExamApp.Application.Features.Users.Dto;

namespace ExamApp.Application.Features.Exams.Dto
{
    public record ExamWithInstructorResponseDto(
        int ExamId,
        string Title,
        string Description,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate,
        int Duration,
        UserResponseDto Instructor
    );
}

