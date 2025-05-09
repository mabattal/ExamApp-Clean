using System.Net;
using AutoMapper;
using ExamApp.Application.Contracts;
using ExamApp.Application.Contracts.Persistence;
using ExamApp.Application.Features.Answers;
using ExamApp.Application.Features.ExamResults.Dto;
using ExamApp.Application.Features.Exams;
using ExamApp.Application.Features.Questions;
using ExamApp.Application.Features.Users;
using ExamApp.Domain.Entities;

namespace ExamApp.Application.Features.ExamResults
{
    public class ExamResultService(
        IExamResultRepository examResultRepository,
        IQuestionService questionService,
        IAnswerService answerService,
        IExamService examService,
        IUserService userService,
        IUnitOfWork unitOfWork,
        IDateTimeUtcConversionService dateTimeService,
        IMapper mapper) : IExamResultService
    {
        public async Task<ServiceResult<ExamResultResponseDto?>> GetByIdAsync(int id)
        {
            var examResult = await examResultRepository.GetByIdAsync(id);
            if (examResult is null)
            {
                return ServiceResult<ExamResultResponseDto>.Fail("Exam result not found.", HttpStatusCode.NotFound)!;
            }

            if (examResult.CompletionDate is null)
            {
                return ServiceResult<ExamResultResponseDto>.Fail("Exam result has not been submitted yet.")!;
            }

            examResult.StartDate = dateTimeService.ConvertFromUtc(examResult.StartDate);
            examResult.CompletionDate = dateTimeService.ConvertFromUtc(examResult.CompletionDate.Value);
            var examResultAsDto = mapper.Map<ExamResultResponseDto>(examResult);
            examResult.StartDate = dateTimeService.ConvertToUtc(examResult.StartDate);
            examResult.CompletionDate = dateTimeService.ConvertToUtc(examResult.CompletionDate.Value);

            return ServiceResult<ExamResultResponseDto>.Success(examResultAsDto)!;
        }

        public async Task<ServiceResult<ExamResultResponseDto?>> GetByUserIdAndExamId(int userId, int examId)
        {
            var examResult = await examResultRepository.GetByUserIdAndExamIdAsync(userId, examId);
            if (examResult is null)
            {
                return ServiceResult<ExamResultResponseDto>.Fail("Exam result not found.", HttpStatusCode.NotFound)!;
            }

            if (examResult.CompletionDate is null)
            {
                return ServiceResult<ExamResultResponseDto>.Fail("Exam result has not been submitted yet.")!;
            }

            examResult.StartDate = dateTimeService.ConvertFromUtc(examResult.StartDate);
            examResult.CompletionDate = dateTimeService.ConvertFromUtc(examResult.CompletionDate.Value);
            var examResultAsDto = mapper.Map<ExamResultResponseDto>(examResult);
            examResult.StartDate = dateTimeService.ConvertToUtc(examResult.StartDate);
            examResult.CompletionDate = dateTimeService.ConvertToUtc(examResult.CompletionDate.Value);

            return ServiceResult<ExamResultResponseDto>.Success(examResultAsDto)!;
        }

        public async Task<ServiceResult<ExamResultResponseDto?>> GetByUserAndExam(int userId, int examId)
        {
            var examResult = await examResultRepository.GetByUserIdAndExamIdAsync(userId, examId);
            if (examResult is null)
            {
                return ServiceResult<ExamResultResponseDto>.Fail("Exam result not found.", HttpStatusCode.NotFound)!;
            }

            var examResultAsDto = mapper.Map<ExamResultResponseDto>(examResult);
            return ServiceResult<ExamResultResponseDto>.Success(examResultAsDto)!;
        }

        public async Task<ServiceResult> StartExamAsync(int examId, int userId)
        {
            var exam = await examService.GetByIdAsync(examId);
            if (exam.IsFail)
            {
                return ServiceResult.Fail(exam.ErrorMessage!, exam.Status);
            }

            if (exam.Data!.StartDate > DateTimeOffset.UtcNow)
            {
                return ServiceResult.Fail("Exam has not started yet.");
            }

            if (exam.Data.EndDate < DateTimeOffset.UtcNow)
            {
                return ServiceResult.Fail("Exam has already ended.");
            }

            var existingResult = await examResultRepository.AnyAsync(x => x.ExamId == examId && x.UserId == userId);
            if (existingResult)
            {
                return ServiceResult.Fail("Exam already started.");
            }

            var questions = await questionService.GetByExamIdAsync(examId);
            if (questions.IsFail)
            {
                return ServiceResult.Fail(questions.ErrorMessage!, questions.Status);
            }


            var examResult = new ExamResult()
            {
                UserId = userId,
                ExamId = examId,
                StartDate = DateTimeOffset.UtcNow,
                TotalQuestions = questions.Data!.Count
            };
            await examResultRepository.AddAsync(examResult);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> SubmitExamAsync(int examId, int userId)
        {
            var exam = await examService.GetByIdAsync(examId);
            if (exam.IsFail)
            {
                return ServiceResult.Fail(exam.ErrorMessage!, exam.Status);
            }

            var existingResult = await examResultRepository.AnyAsync(x => x.ExamId == examId && x.UserId == userId);
            if (!existingResult)
            {
                return ServiceResult.Fail("Exam result is not found.", HttpStatusCode.NotFound);
            }

            var examResult = await examResultRepository.GetByUserIdAndExamIdAsync(userId, examId);
            if (examResult is { Score: not null, CompletionDate: not null, CorrectAnswers: not null, IncorrectAnswers: not null, Duration: not null })
            {
                return ServiceResult.Fail("Exam has already been submitted.");
            }

            var questions = await questionService.GetByExamIdAsync(examId);
            if (questions.IsFail)
            {
                return ServiceResult.Fail(questions.ErrorMessage!, questions.Status);
            }

            var totalQuestions = questions.Data!.Count;
            var correctAnswers = 0;
            var incorrectAnswers = 0;
            var emptyAnswers = totalQuestions;
            decimal score = 0;
            var answers = await answerService.GetByUserAndExamAsync(userId, examId);
            if (answers.IsSuccess)
            {
                correctAnswers = answers.Data!.Count(a => a.IsCorrect == true);
                incorrectAnswers = answers.Data!.Count(a => a.IsCorrect == false);
                emptyAnswers = totalQuestions - (correctAnswers + incorrectAnswers);
                score = (correctAnswers / (decimal)totalQuestions) * 100;
            }
            var duration = (int)Math.Ceiling((DateTimeOffset.UtcNow - examResult!.StartDate).TotalMinutes);

            examResult.Score = score;
            examResult.CompletionDate = DateTimeOffset.UtcNow;
            examResult.Duration = duration;
            examResult.CorrectAnswers = correctAnswers;
            examResult.IncorrectAnswers = incorrectAnswers;
            examResult.EmptyAnswers = emptyAnswers;

            examResultRepository.Update(examResult);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> AutoSubmitExpiredExamsAsync()
        {
            var expiredResults = await examResultRepository.GetExpiredResultsAsync();

            if (!expiredResults.Any())
            {
                return ServiceResult.Fail("No expired exams found.");
            }

            foreach (var result in expiredResults)
            {
                var submitResult = await SubmitExamAsync(result.ExamId, result.UserId);
                if (submitResult.IsFail)
                {
                    return submitResult;
                }
            }

            return ServiceResult.Success();
        }

        public async Task<ServiceResult<List<ExamResultResponseDto>>> GetByUserIdAsync(int userId)
        {
            var user = await userService.GetByIdOrEmailAsync(userId.ToString());
            if (user.IsFail)
            {
                return ServiceResult<List<ExamResultResponseDto>>.Fail(user.ErrorMessage!, user.Status);
            }

            var examResults = await examResultRepository.GetByUserIdAsync(userId);
            if (!examResults.Any())
            {
                return ServiceResult<List<ExamResultResponseDto>>.Fail("No completed exam results found.", HttpStatusCode.NotFound);
            }

            var examResultsAsDto = examResults
                .Where(x => x.CompletionDate != null)
                .Select(x =>
            {
                x.StartDate = dateTimeService.ConvertFromUtc(x.StartDate);
                x.CompletionDate = dateTimeService.ConvertFromUtc(x.CompletionDate!.Value);
                var dto = mapper.Map<ExamResultResponseDto>(x);
                x.StartDate = dateTimeService.ConvertToUtc(x.StartDate);
                x.CompletionDate = dateTimeService.ConvertToUtc(x.CompletionDate!.Value);
                return dto;
            }).ToList();

            return ServiceResult<List<ExamResultResponseDto>>.Success(examResultsAsDto);
        }

        public async Task<ServiceResult<ExamResultStatisticsResponseDto>> GetStatisticsByExamAsync(int examId)
        {
            var examResultsExist = await examResultRepository.AnyAsync(x => x.ExamId == examId);
            if (!examResultsExist)
            {
                return ServiceResult<ExamResultStatisticsResponseDto>.Fail("No exam results found.", HttpStatusCode.NotFound);
            }

            var averageScore = await examResultRepository.GetAverageScoreByExamAsync(examId);
            var maxScore = await examResultRepository.GetMaxScoreByExamAsync(examId);
            var minScore = await examResultRepository.GetMinScoreByExamAsync(examId);
            var studentCount = await examResultRepository.GetExamCountByExamAsync(examId);

            var statisticsAsDto = new ExamResultStatisticsResponseDto(
                studentCount,
                averageScore,
                maxScore,
                minScore
            );

            return ServiceResult<ExamResultStatisticsResponseDto>.Success(statisticsAsDto);
        }

        //exam id'ye göre sonuçları getirir, user bilgileri ile birlikte
        public async Task<ServiceResult<List<ExamResultResponseDto>>> GetByExamIdAsync(int examId)
        {
            var examResults = await examResultRepository.GetByExamIdAsync(examId);
            if (!examResults.Any())
            {
                return ServiceResult<List<ExamResultResponseDto>>.Fail("No exam results found.", HttpStatusCode.NotFound);
            }
            var examResultsAsDto = examResults
                .Select(x =>
                {
                    x.StartDate = dateTimeService.ConvertFromUtc(x.StartDate);
                    x.CompletionDate = dateTimeService.ConvertFromUtc(x.CompletionDate!.Value);
                    var dto = mapper.Map<ExamResultResponseDto>(x);
                    x.StartDate = dateTimeService.ConvertToUtc(x.StartDate);
                    x.CompletionDate = dateTimeService.ConvertToUtc(x.CompletionDate!.Value);
                    return dto;
                }).ToList();
            return ServiceResult<List<ExamResultResponseDto>>.Success(examResultsAsDto);
        }


    }
}
