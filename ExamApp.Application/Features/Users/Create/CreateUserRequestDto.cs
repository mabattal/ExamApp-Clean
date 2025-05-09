using ExamApp.Domain.Enums;

namespace ExamApp.Application.Features.Users.Create
{
    public record CreateUserRequestDto(
        string FullName,
        string Email,
        UserRole Role,
        string Password);
}
