namespace ExamApp.Application.Features.Questions.Create
{
    public record CreateQuestionRequestDto(
        int ExamId,
        string QuestionText,
        string OptionA,
        string OptionB,
        string OptionC,
        string OptionD,
        string CorrectAnswer
        );
}
