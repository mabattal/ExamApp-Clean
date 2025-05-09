using AutoMapper;
using ExamApp.Application.Features.Exams.Create;
using ExamApp.Application.Features.Exams.Dto;
using ExamApp.Application.Features.Exams.Update;
using ExamApp.Domain.Entities;

namespace ExamApp.Application.Features.Exams
{
    public class ExamMappingProfile : Profile
    {
        public ExamMappingProfile()
        {
            CreateMap<CreateExamRequestDto, Exam>();
            CreateMap<UpdateExamRequestDto, Exam>();
            CreateMap<Exam, ExamWithQuestionsResponseDto>();
            CreateMap<Exam, ExamWithInstructorResponseDto>();
            CreateMap<Exam, ExamWithDetailsResponseDto>();
        }
    }
}
