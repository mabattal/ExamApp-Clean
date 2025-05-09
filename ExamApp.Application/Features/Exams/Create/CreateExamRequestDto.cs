namespace ExamApp.Application.Features.Exams.Create
{
    public record CreateExamRequestDto(
        string Title,
        string Description,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate,
        int Duration
    );
}
