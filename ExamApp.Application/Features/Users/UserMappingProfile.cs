using AutoMapper;
using ExamApp.Application.Features.Users.Create;
using ExamApp.Application.Features.Users.Dto;
using ExamApp.Application.Features.Users.Update;
using ExamApp.Domain.Entities;

namespace ExamApp.Application.Features.Users
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<CreateUserRequestDto, User>();
            CreateMap<UpdateUserRequestDto, User>();
        }
    }
}
