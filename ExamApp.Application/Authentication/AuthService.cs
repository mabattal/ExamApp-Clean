using System.Net;
using ExamApp.Application.Authentication.Dto;
using ExamApp.Application.Contracts;
using ExamApp.Application.Contracts.Persistence;
using ExamApp.Domain.Entities;
using ExamApp.Domain.Enums;

namespace ExamApp.Application.Authentication
{
    public class AuthService(IUserRepository userRepository, JwtService jwtService, IUnitOfWork unitOfWork) : IAuthService
    {
        public async Task<ServiceResult<UserTokenResponseDto>> ValidateUserAsync(string email, string password)
        {
            var user = await userRepository.GetByEmailAsync(email);
            if (user is null)
            {
                return ServiceResult<UserTokenResponseDto>.Fail("Email not found.", HttpStatusCode.Unauthorized);
            }
            if (!PasswordHasher.Verify(password, user.Password))
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

            var hashedPassword = PasswordHasher.Hash(request.Password);

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
