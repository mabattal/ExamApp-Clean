namespace ExamApp.Application.Features.Exams.Update
{
    public record UpdateExamRequestDto(
        string Title,
        string Description,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate,
        int Duration
    );
}
