namespace ExamApp.Application.Features.Answers.Create
{
    public record CreateAnswerRequestDto(
        int ExamId, 
        int QuestionId, 
        string SelectedAnswer
        );
}
