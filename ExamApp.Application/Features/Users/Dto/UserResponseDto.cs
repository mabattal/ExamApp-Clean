using ExamApp.Domain.Enums;

namespace ExamApp.Application.Features.Users.Dto
{
    public record UserResponseDto(
        int UserId,
        string? FullName,
        string Email,
        UserRole Role,
        bool IsDeleted
    );
}