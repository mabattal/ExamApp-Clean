using FluentValidation;

namespace ExamApp.Application.Features.Exams.Create
{
    public class CreateExamRequestValidator : AbstractValidator<CreateExamRequestDto>
    {
        public CreateExamRequestValidator()
        {
            RuleFor(x=> x.Title)
                .NotEmpty().WithMessage("Title required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate required.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("EndDate required.");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Duration required and must be greater than 0.");
        }
    }
}
