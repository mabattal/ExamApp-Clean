namespace ExamApp.Application.Features.ExamResults.Dto
{
    public record ExamResultStatisticsResponseDto(
        int StudentCount,
        decimal AverageScore,
        decimal MaxScore,
        decimal MinScore
        );
}
