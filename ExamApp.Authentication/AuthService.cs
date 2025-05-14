using System.Net;
using ExamApp.Application;
using ExamApp.Application.Contracts.Authentication;
using ExamApp.Application.Contracts.Authentication.Dto;
using ExamApp.Application.Contracts.Persistence;
using ExamApp.Domain.Entities;
using ExamApp.Domain.Enums;

namespace ExamApp.Authentication
{
    public class AuthService(IUserRepository userRepository, IJwtService jwtService, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork) : IAuthService
    {
        public async Task<ServiceResult<UserTokenResponseDto>> ValidateUserAsync(string email, string password)
        {
            var user = await userRepository.GetByEmailAsync(email);
            if (user is null)
            {
                return ServiceResult<UserTokenResponseDto>.Fail("Email not found.", HttpStatusCode.Unauthorized);
            }
            if (!passwordHasher.Verify(password, user.Password))
            {
                return ServiceResult<UserTokenResponseDto>.Fail("Invalid password.", HttpStatusCode.Unauthorized);
            }

            var token = jwtService.GenerateToken(user.UserId, user.Role.ToString());

            return ServiceResult<UserTokenResponseDto>.Success(new UserTokenResponseDto(token, user.Role.ToString(), user.FullName!, user.Email, user.UserId));
        }

        public async Task<ServiceResult> RegisterAsync(RegisterUserRequestDto request)
        {
            var existingUser = await userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return ServiceResult.Fail("This email is already in use.", HttpStatusCode.BadRequest);
            }

            var hashedPassword = passwordHasher.Hash(request.Password);

            var newUser = new User
            {
                Email = request.Email,
                Password = hashedPassword,
                Role = UserRole.Student,
                IsDeleted = false
            };

            await userRepository.AddAsync(newUser);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.Created);
        }
    }
}
