using AutoMapper;
using ExamApp.Application.Features.ExamResults.Dto;
using ExamApp.Domain.Entities;

namespace ExamApp.Application.Features.ExamResults
{
    public class ExamResultMappingProfile : Profile
    {
        public ExamResultMappingProfile()
        {
            CreateMap<ExamResult, ExamResultResponseDto>();

        }
    }
}
