namespace ExamApp.Application.Contracts.Authentication.Dto
{
    public record RegisterUserRequestDto(
        string Email,
        string Password);
}
