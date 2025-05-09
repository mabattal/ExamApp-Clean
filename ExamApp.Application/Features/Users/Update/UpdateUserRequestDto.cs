using ExamApp.Domain.Enums;

namespace ExamApp.Application.Features.Users.Update
{
    public record UpdateUserRequestDto(
        string FullName,
        string Email,
        UserRole Role,
        string Password
        );
}
