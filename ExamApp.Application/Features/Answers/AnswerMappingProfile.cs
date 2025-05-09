using AutoMapper;
using ExamApp.Application.Features.Answers.Create;
using ExamApp.Application.Features.Answers.Dto;
using ExamApp.Application.Features.Answers.Update;
using ExamApp.Domain.Entities;

namespace ExamApp.Application.Features.Answers
{
    public class AnswerMappingProfile : Profile
    {
        public AnswerMappingProfile()
        {
            CreateMap<Answer, AnswerResponseDto>();
            CreateMap<CreateAnswerRequestDto, Answer>();
            CreateMap<UpdateAnswerRequestDto, Answer>();
        }
    }
}
