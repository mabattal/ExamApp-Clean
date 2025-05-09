using AutoMapper;
using ExamApp.Application.Features.Questions.Create;
using ExamApp.Application.Features.Questions.Dto;
using ExamApp.Application.Features.Questions.Update;
using ExamApp.Domain.Entities;

namespace ExamApp.Application.Features.Questions
{
    public class QuestionMappingProfile:Profile
    {
        public QuestionMappingProfile()
        {
            CreateMap<Question, QuestionResponseDto>();
            CreateMap<CreateQuestionRequestDto, Question>();
            CreateMap<UpdateQuestionRequestDto, Question>();
            CreateMap<Question, QuestionResponseWithoutCorrectAnswerDto>();
        }
    }
}
