using System.Net;
using AutoMapper;
using ExamApp.Application.Contracts.Authentication;
using ExamApp.Application.Contracts.Persistence;
using ExamApp.Application.Features.Users.Create;
using ExamApp.Application.Features.Users.Dto;
using ExamApp.Application.Features.Users.Update;
using ExamApp.Domain.Entities;
using ExamApp.Domain.Enums;

namespace ExamApp.Application.Features.Users
{
    public class UserService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPasswordHasher passwordHasher) : IUserService
    {
        public async Task<ServiceResult<List<UserResponseDto>>> GetAllAsync()
        {
            var users = await userRepository.GetAllAsync();
            var userAsDto = mapper.Map<List<UserResponseDto>>(users);

            //manuel mapping
            //var userAsDto = users.Select(u => new UserResponseDto(u.UserId, u.FullName, u.Email, u.Role, u.IsDeleted)).ToList();

            return ServiceResult<List<UserResponseDto>>.Success(userAsDto);
        }

        public async Task<ServiceResult<List<UserResponseDto>>> GetPagedAllAsync(int pageNumber, int pageSize)
        {
            // pageNumber - pageSize
            // 1 - 10 => 0, 10  kayıt    skip(0).take(10)
            // 2 - 10 => 11, 20 kayıt    skip(10).take(10)
            // 3 - 10 => 21, 30 kayıt    skip(20).take(10)
            // 4 - 10 => 31, 40 kayıt    skip(30).take(10)

            var users = await userRepository.GetAllPagedAsync(pageNumber, pageSize);
            var userAsDto = mapper.Map<List<UserResponseDto>>(users);
            return ServiceResult<List<UserResponseDto>>.Success(userAsDto);
        }

        public async Task<ServiceResult<UserResponseDto?>> GetByIdOrEmailAsync(string value)
        {
            User? user;

            if (int.TryParse(value, out int id))
            {
                var anyUser = await userRepository.AnyAsync(u => u.UserId == id);
                if (!anyUser)
                {
                    return ServiceResult<UserResponseDto?>.Fail("User not found.", HttpStatusCode.NotFound);
                }
                user = await userRepository.GetByIdAsync(id);
            }
            else
            {
                var anyUser = await userRepository.AnyAsync(u => u.Email == value);
                if (!anyUser)
                {
                    return ServiceResult<UserResponseDto?>.Fail("User not found.", HttpStatusCode.NotFound);
                }

                user = await userRepository.GetByEmailAsync(value);
            }

            if (user is null)
            {
                return ServiceResult<UserResponseDto?>.Fail("User not found.", HttpStatusCode.NotFound);
            }

            var userDto = mapper.Map<UserResponseDto>(user);
            return ServiceResult<UserResponseDto?>.Success(userDto);
        }

        public async Task<ServiceResult<UserResponseDto?>> GetInstructorByIdAsync(int id)
        {
            var instructor = await userRepository.GetInstructorByIdAsync(id, UserRole.Instructor);
            if (instructor is null)
            {
                return ServiceResult<UserResponseDto>.Fail("Instructor not found or not authorized.", HttpStatusCode.NotFound)!;
            }
            var userAsDto = mapper.Map<UserResponseDto>(instructor);
            return ServiceResult<UserResponseDto>.Success(userAsDto)!;
        }

        public async Task<ServiceResult<CreateUserResponseDto>> AddAsync(CreateUserRequestDto createUserRequest)
        {
            var existingUser = await userRepository.AnyAsync(u => u.Email == createUserRequest.Email);
            if (existingUser) {
                return ServiceResult<CreateUserResponseDto>.Fail("E-mail address already exists", HttpStatusCode.BadRequest);
            }

            var hashedPassword = passwordHasher.Hash(createUserRequest.Password);
            var user = mapper.Map<User>(createUserRequest);
            user.IsDeleted = false;
            user.Password = hashedPassword;

            await userRepository.AddAsync(user);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult<CreateUserResponseDto>.Success(new CreateUserResponseDto(user.UserId));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateUserRequestDto updateUserRequest)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user is null)
            {
                return ServiceResult.Fail("User not found", HttpStatusCode.NotFound);
            }

            var existingUser = await userRepository.AnyAsync(u => u.Email == updateUserRequest.Email && u.UserId != id);
            if (existingUser)
            {
                return ServiceResult.Fail("E-mail address already exists", HttpStatusCode.BadRequest);
            }

            var hashedPassword = passwordHasher.Hash(updateUserRequest.Password);
            user = mapper.Map(updateUserRequest, user);
            user.Password = hashedPassword;

            userRepository.Update(user);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user is null)
            {
                return ServiceResult.Fail("User not found", HttpStatusCode.NotFound);
            }
            user.IsDeleted = true;

            userRepository.Update(user);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<List<UserResponseDto>>> GetByRole(UserRole role)
        {
            var users = await userRepository.GetByRoleAsync(role);
            var userAsDto = mapper.Map<List<UserResponseDto>>(users);
            return ServiceResult<List<UserResponseDto>>.Success(userAsDto);
        }
    }
}
