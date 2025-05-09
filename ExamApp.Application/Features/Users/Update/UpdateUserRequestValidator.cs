using FluentValidation;

namespace ExamApp.Application.Features.Users.Update
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequestDto>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.FullName)
                .MaximumLength(100).WithMessage("FullName must not exceed 100 characters.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email required.")
                .EmailAddress().WithMessage("Email must be a valid email address.")
                .MaximumLength(150).WithMessage("Email must not exceed 150 characters.");
            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Role must be a valid UserRole.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password required.");
        }
    }
}
