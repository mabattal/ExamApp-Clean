using ExamApp.Application.Features.Users.Create;
using ExamApp.Application.Features.Users.Dto;
using ExamApp.Application.Features.Users.Update;
using ExamApp.Domain.Enums;

namespace ExamApp.Application.Features.Users
{
    public interface IUserService
    {
        Task<ServiceResult<List<UserResponseDto>>> GetAllAsync();
        Task<ServiceResult<List<UserResponseDto>>> GetPagedAllAsync(int pageNumber, int pageSize);
        Task<ServiceResult<UserResponseDto?>> GetByIdOrEmailAsync(string value);
        Task<ServiceResult<UserResponseDto?>> GetInstructorByIdAsync(int id);
        Task<ServiceResult<CreateUserResponseDto>> AddAsync(CreateUserRequestDto createUserRequest);
        Task<ServiceResult> UpdateAsync(int id, UpdateUserRequestDto updateUserRequest);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult<List<UserResponseDto>>> GetByRole(UserRole role);

    }
}
