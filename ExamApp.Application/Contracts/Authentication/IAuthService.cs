using ExamApp.Application.Contracts.Authentication.Dto;

namespace ExamApp.Application.Contracts.Authentication
{
    public interface IAuthService
    {
        Task<ServiceResult<UserTokenResponseDto>> ValidateUserAsync(string email, string password);
        Task<ServiceResult> RegisterAsync(RegisterUserRequestDto request);
    }

}
