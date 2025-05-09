using FluentValidation;

namespace ExamApp.Application.Features.Questions.Create
{
    public class CreateQuestionRequestValidator : AbstractValidator<CreateQuestionRequestDto>
    {
        public CreateQuestionRequestValidator()
        {
            RuleFor(x => x.ExamId)
                .GreaterThan(0).WithMessage("ExamId required and must be greater than 0.");
            RuleFor(x => x.QuestionText)
                .NotEmpty().WithMessage("QuestionText required.")
                .MaximumLength(1000).WithMessage("QuestionText must not exceed 1000 characters.");
            RuleFor(x => x.OptionA)
                .NotEmpty().WithMessage("OptionA required.")
                .MaximumLength(200).WithMessage("OptionA must not exceed 200 characters.");
            RuleFor(x => x.OptionB)
                .NotEmpty().WithMessage("OptionB required.")
                .MaximumLength(200).WithMessage("OptionB must not exceed 200 characters.");
            RuleFor(x => x.OptionC)
                .NotEmpty().WithMessage("OptionC required.")
                .MaximumLength(200).WithMessage("OptionC must not exceed 200 characters.");
            RuleFor(x => x.OptionD)
                .NotEmpty().WithMessage("OptionD required.")
                .MaximumLength(200).WithMessage("OptionD must not exceed 200 characters.");
            RuleFor(x => x.CorrectAnswer)
                .NotEmpty().WithMessage("CorrectAnswer required.")
                .MaximumLength(200).WithMessage("CorrectAnswer must not exceed 200 character.");
        }
    }
}
