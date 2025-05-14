namespace ExamApp.Application.Contracts.Authentication.Dto
{
    public record UserTokenResponseDto(
        string Token,
        string Role,
        string FullName,
        string Email,
        int UserId);
}
