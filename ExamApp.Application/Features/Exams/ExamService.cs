using System.Net;
using AutoMapper;
using ExamApp.Application.Contracts;
using ExamApp.Application.Contracts.Caching;
using ExamApp.Application.Contracts.Persistence;
using ExamApp.Application.Features.Exams.Create;
using ExamApp.Application.Features.Exams.Dto;
using ExamApp.Application.Features.Exams.Update;
using ExamApp.Application.Features.Users;
using ExamApp.Domain.Entities;

namespace ExamApp.Application.Features.Exams
{
    public class ExamService(
        IExamRepository examRepository, 
        IUserService userService, 
        IUnitOfWork unitOfWork, 
        IDateTimeUtcConversionService dateTimeService,
        IMapper mapper,
        ICacheService cacheService) : IExamService
    {
        private const string GetByInstructorCacheKey = "GetByInstructorCacheKey";
        private const string ActiveExamsCacheKey = "ActiveExamsCacheKey";
        private const string PastExamsCacheKey = "PastExamsCacheKey";
        private const string UpcomingExamsCacheKey = "UpcomingExamsCacheKey";

        public async Task<ServiceResult<CreateExamResponseDto>> AddAsync(CreateExamRequestDto examRequest, int userId)
        {
            var instructor = await userService.GetInstructorByIdAsync(userId);
            if (instructor.IsFail)
            {
                return ServiceResult<CreateExamResponseDto>.Fail(instructor.ErrorMessage!, instructor.Status);
            }

            if (examRequest.StartDate >= examRequest.EndDate)
            {
                return ServiceResult<CreateExamResponseDto>.Fail("Start date cannot be greater than end date.", HttpStatusCode.BadRequest);
            }

            if (examRequest.Duration > (examRequest.EndDate - examRequest.StartDate).TotalMinutes)
            {
                return ServiceResult<CreateExamResponseDto>.Fail("Duration cannot be greater than the difference between start and end date.", HttpStatusCode.BadRequest);
            }

            if (examRequest.StartDate < DateTimeOffset.UtcNow || examRequest.EndDate < DateTimeOffset.UtcNow)
            {
                return ServiceResult<CreateExamResponseDto>.Fail("Start date and end date must be greater than the current date.", HttpStatusCode.BadRequest);
            }

            var exam = mapper.Map<Exam>(examRequest);
            exam.CreatedBy = userId;
            exam.StartDate = dateTimeService.ConvertToUtc(examRequest.StartDate);
            exam.EndDate = dateTimeService.ConvertToUtc(examRequest.EndDate);
            exam.IsDeleted = false;

            await examRepository.AddAsync(exam);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult<CreateExamResponseDto>.Success(new CreateExamResponseDto(exam.ExamId));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateExamRequestDto examRequest, int userId)
        {
            var exam = await examRepository.GetByIdAsync(id);
            if (exam is null)
            {
                return ServiceResult.Fail("Exam not found", HttpStatusCode.NotFound);
            }

            var instructor = await userService.GetInstructorByIdAsync(userId);
            if (instructor.IsFail)
            {
                return ServiceResult.Fail(instructor.ErrorMessage!, instructor.Status);
            }

            if (exam.CreatedBy != userId)
            {
                return ServiceResult.Fail("You are not authorized to update this exam.", HttpStatusCode.Forbidden);
            }

            if (examRequest.StartDate >= examRequest.EndDate)
            {
                return ServiceResult.Fail("Start date cannot be greater than end date.", HttpStatusCode.BadRequest);
            }

            if (examRequest.Duration > (examRequest.EndDate - examRequest.StartDate).TotalMinutes)
            {
                return ServiceResult.Fail("Duration cannot be greater than the difference between start and end date.", HttpStatusCode.BadRequest);
            }

            if (examRequest.StartDate < DateTimeOffset.UtcNow || examRequest.EndDate < DateTimeOffset.UtcNow)
            {
                return ServiceResult.Fail("Start date and end date must be greater than the current date.", HttpStatusCode.BadRequest);
            }

            mapper.Map(examRequest, exam);
            exam.StartDate = dateTimeService.ConvertToUtc(examRequest.StartDate);
            exam.EndDate = dateTimeService.ConvertToUtc(examRequest.EndDate);

            examRepository.Update(exam);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id, int userId)
        {
            var exam = await examRepository.GetByIdAsync(id);
            if (exam is null)
            {
                return ServiceResult.Fail("Exam not found", HttpStatusCode.NotFound);
            }
            if (exam.CreatedBy != userId)
            {
                return ServiceResult.Fail("You are not authorized to delete this exam.", HttpStatusCode.Forbidden);
            }
            exam.IsDeleted = true;

            examRepository.Update(exam);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<List<ExamWithQuestionsResponseDto>>> GetByInstructorAsync(int instructorId)
        {
            var instructor = await userService.GetInstructorByIdAsync(instructorId);
            if (instructor.IsFail)
            {
                return ServiceResult<List<ExamWithQuestionsResponseDto>>.Fail(instructor.ErrorMessage!, instructor.Status);
            }

            var examResponse = await cacheService.GetAsync<List<ExamWithQuestionsResponseDto>>(GetByInstructorCacheKey + instructorId);
            if (examResponse is not null)
            {
                return ServiceResult<List<ExamWithQuestionsResponseDto>>.Success(examResponse);
            }

            var exams = await examRepository.GetByInstructorAsync(instructorId);
            var examAsDto = exams.Select(e =>
            {
                e.StartDate = dateTimeService.ConvertFromUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertFromUtc(e.EndDate);
                var dto = mapper.Map<ExamWithQuestionsResponseDto>(e);
                e.StartDate = dateTimeService.ConvertToUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertToUtc(e.EndDate);
                return dto;
            }).ToList();

            await cacheService.AddAsync(GetByInstructorCacheKey + instructorId, examAsDto, TimeSpan.FromMinutes(1));
            return ServiceResult<List<ExamWithQuestionsResponseDto>>.Success(examAsDto);
        }

        public async Task<ServiceResult<List<ExamWithInstructorResponseDto>>> GetActiveExamsAsync()
        {
            var examResponse = await cacheService.GetAsync<List<ExamWithInstructorResponseDto>>(ActiveExamsCacheKey);
            if (examResponse is not null)
            {
                return ServiceResult<List<ExamWithInstructorResponseDto>>.Success(examResponse);
            }

            var exams = await examRepository.GetActiveExamsAsync();
            if (!exams.Any())
            {
                return ServiceResult<List<ExamWithInstructorResponseDto>>.Fail("There is no active exam.", HttpStatusCode.NotFound);
            }

            var examAsDto = exams.Select(e =>
            {
                e.StartDate = dateTimeService.ConvertFromUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertFromUtc(e.EndDate);
                var dto = mapper.Map<ExamWithInstructorResponseDto>(e);
                e.StartDate = dateTimeService.ConvertToUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertToUtc(e.EndDate);
                return dto;
            }).ToList();

            await cacheService.AddAsync(ActiveExamsCacheKey, examAsDto, TimeSpan.FromMinutes(1));
            return ServiceResult<List<ExamWithInstructorResponseDto>>.Success(examAsDto);
        }

        public async Task<ServiceResult<List<ExamWithInstructorResponseDto>>> GetPastExamsAsync()
        {
            var examResponse = await cacheService.GetAsync<List<ExamWithInstructorResponseDto>>(PastExamsCacheKey);
            if (examResponse is not null)
            {
                return ServiceResult<List<ExamWithInstructorResponseDto>>.Success(examResponse);
            }

            var exams = await examRepository.GetPastExamsAsync();
            if (!exams.Any())
            {
                return ServiceResult<List<ExamWithInstructorResponseDto>>.Fail("There is no past exam.", HttpStatusCode.NotFound);
            }

            var examAsDto = exams.Select(e =>
            {
                e.StartDate = dateTimeService.ConvertFromUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertFromUtc(e.EndDate);
                var dto = mapper.Map<ExamWithInstructorResponseDto>(e);
                e.StartDate = dateTimeService.ConvertToUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertToUtc(e.EndDate);
                return dto;
            }).ToList();

            await cacheService.AddAsync(PastExamsCacheKey, examAsDto, TimeSpan.FromMinutes(1));
            return ServiceResult<List<ExamWithInstructorResponseDto>>.Success(examAsDto);
        }

        public async Task<ServiceResult<List<ExamWithInstructorResponseDto>>> GetUpcomingExamsAsync()
        {
            var examResponse = await cacheService.GetAsync<List<ExamWithInstructorResponseDto>>(UpcomingExamsCacheKey);
            if (examResponse is not null)
            {
                return ServiceResult<List<ExamWithInstructorResponseDto>>.Success(examResponse);
            }

            var exams = await examRepository.GetUpcomingExamsAsync();
            if (!exams.Any())
            {
                return ServiceResult<List<ExamWithInstructorResponseDto>>.Fail("There is no upcoming exam.", HttpStatusCode.NotFound);
            }

            var examAsDto = exams.Select(e =>
            {
                e.StartDate = dateTimeService.ConvertFromUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertFromUtc(e.EndDate);
                var dto = mapper.Map<ExamWithInstructorResponseDto>(e);
                e.StartDate = dateTimeService.ConvertToUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertToUtc(e.EndDate);
                return dto;
            }).ToList();

            await cacheService.AddAsync(UpcomingExamsCacheKey, examAsDto, TimeSpan.FromMinutes(1));
            return ServiceResult<List<ExamWithInstructorResponseDto>>.Success(examAsDto);
        }

        public async Task<ServiceResult<ExamWithDetailsResponseDto?>> GetByIdAsync(int id)
        {
            var exam = await examRepository.GetExamWithDetailsAsync(id);
            if (exam is null)
            {
                return ServiceResult<ExamWithDetailsResponseDto?>.Fail("Exam not found", HttpStatusCode.NotFound);
            }

            exam.StartDate = dateTimeService.ConvertFromUtc(exam.StartDate);
            exam.EndDate = dateTimeService.ConvertFromUtc(exam.EndDate);
            var examAsDto = mapper.Map<ExamWithDetailsResponseDto>(exam);
            exam.StartDate = dateTimeService.ConvertToUtc(exam.StartDate);
            exam.EndDate = dateTimeService.ConvertToUtc(exam.EndDate);

            return ServiceResult<ExamWithDetailsResponseDto?>.Success(examAsDto);
        }
    }
}
