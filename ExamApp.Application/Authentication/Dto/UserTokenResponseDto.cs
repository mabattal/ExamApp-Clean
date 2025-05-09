namespace ExamApp.Application.Authentication.Dto
{
    public record UserTokenResponseDto(
        string Token,
        string Role,
        string FullName,
        string Email,
        int UserId);
}
