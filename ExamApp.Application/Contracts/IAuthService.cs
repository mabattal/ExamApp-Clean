using ExamApp.Application.Authentication.Dto;

namespace ExamApp.Application.Contracts
{
    public interface IAuthService
    {
        Task<ServiceResult<UserTokenResponseDto>> ValidateUserAsync(string email, string password);
        Task<ServiceResult> RegisterAsync(RegisterUserRequestDto request);
    }

}
