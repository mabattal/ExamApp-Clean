namespace ExamApp.Application.Features.Questions.Update
{
    public record UpdateQuestionRequestDto(
        string QuestionText,
        string OptionA,
        string OptionB,
        string OptionC,
        string OptionD,
        string CorrectAnswer
        );
}
