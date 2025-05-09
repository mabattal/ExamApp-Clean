namespace ExamApp.Application.Features.Answers.Dto
{
    public record AnswerResponseDto(
        int AnswerId, 
        int UserId, 
        int QuestionId, 
        string SelectedAnswer, 
        bool? IsCorrect, 
        DateTimeOffset CreatedDate);
}
