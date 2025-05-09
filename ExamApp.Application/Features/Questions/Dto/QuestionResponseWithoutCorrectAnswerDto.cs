namespace ExamApp.Application.Features.Questions.Dto
{
    public record QuestionResponseWithoutCorrectAnswerDto(
        int QuestionId,
        int ExamId,
        string QuestionText,
        string OptionA,
        string OptionB,
        string OptionC,
        string OptionD
    );
}
