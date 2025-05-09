using System.Net;
using AutoMapper;
using ExamApp.Application.Contracts;
using ExamApp.Application.Contracts.Persistence;
using ExamApp.Application.Features.Answers.Create;
using ExamApp.Application.Features.Answers.Dto;
using ExamApp.Application.Features.Answers.Update;
using ExamApp.Application.Features.ExamResults;
using ExamApp.Application.Features.Exams;
using ExamApp.Application.Features.Questions;
using ExamApp.Domain.Entities;

namespace ExamApp.Application.Features.Answers
{
    public class AnswerService(
        IAnswerRepository answerRepository, 
        IQuestionService questionService, 
        Lazy<IExamResultService> examResultService, 
        IExamService examService, 
        IUnitOfWork unitOfWork, 
        IDateTimeUtcConversionService dateTimeService,
        IMapper mapper) : IAnswerService
    {
        public async Task<ServiceResult<CreateAnswerResponseDto>> AddAsync(CreateAnswerRequestDto createAnswerRequest, int userId)
        {
            var exam = await examService.GetByIdAsync(createAnswerRequest.ExamId);
            if (exam.IsFail)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail(exam.ErrorMessage!, exam.Status);
            }

            var examResult = await examResultService.Value.GetByUserAndExam(userId, createAnswerRequest.ExamId);
            if (examResult.IsFail)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail("Exam has not been started for this user.", HttpStatusCode.NotFound);
            }

            var question = await questionService.GetByIdAsync(createAnswerRequest.QuestionId);
            if (question.IsFail)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail(question.ErrorMessage!, question.Status);
            }

            if (question.Data!.ExamId != createAnswerRequest.ExamId)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail("Question does not belong to the exam.", HttpStatusCode.BadRequest);
            }

            var existingAnswer = await answerRepository.GetByUserAndQuestionAsync(userId, createAnswerRequest.QuestionId);
            if (existingAnswer is not null)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail("Answer already exists", HttpStatusCode.BadRequest);
            }

            if (exam.Data!.StartDate > DateTimeOffset.Now)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail("Exam has not started yet.", HttpStatusCode.BadRequest);
            }

            // Kullanıcı için sınav süresi mi doldu, yoksa sınav zaten bitti mi?
            var examEndTime = examResult.Data!.StartDate.AddMinutes(exam.Data.Duration);
            var finalExamEndTime = exam.Data.EndDate < examEndTime ? exam.Data.EndDate : examEndTime;
            if (DateTimeOffset.UtcNow > finalExamEndTime)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail("Exam time is up! You cannot submit answers.", HttpStatusCode.BadRequest);
            }

            // `Cevap doğru ise true, yanlış ise false boş ise null olacak şekilde kontrol edilir`
            bool? isCorrect = string.IsNullOrEmpty(createAnswerRequest.SelectedAnswer)
                ? null
                : createAnswerRequest.SelectedAnswer.Equals(question.Data.CorrectAnswer, StringComparison.OrdinalIgnoreCase);

            var answer = mapper.Map<Answer>(createAnswerRequest);
            answer.UserId = userId;
            answer.IsCorrect = isCorrect;

            await answerRepository.AddAsync(answer);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult<CreateAnswerResponseDto>.Success(new CreateAnswerResponseDto(answer.AnswerId));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateAnswerRequestDto request, int userId)
        {
            var answer = await answerRepository.GetByIdWithDetailsAsync(id);
            if (answer is null)
            {
                return ServiceResult.Fail("Answer not found", HttpStatusCode.NotFound);
            }

            if (answer.UserId != userId)
            {
                return ServiceResult.Fail("You are not authorized to update this answer.", HttpStatusCode.Unauthorized);
            }

            var exam = await examService.GetByIdAsync(answer.Question!.ExamId);
            if (exam.IsFail)
            {
                return ServiceResult.Fail(exam.ErrorMessage!, exam.Status);
            }

            var examResult = await examResultService.Value.GetByUserAndExam(answer.UserId, answer.Question.ExamId);
            if (examResult.IsFail)
            {
                return ServiceResult.Fail("Exam has not been started for this user.", HttpStatusCode.NotFound);
            }

            if (examResult.Data!.CompletionDate is not null)
            {
                return ServiceResult.Fail("Exam has already ended", HttpStatusCode.BadRequest);
            }

            var question = await questionService.GetByIdAsync(answer.QuestionId);
            if (question.IsFail)
            {
                return ServiceResult.Fail(question.ErrorMessage!, question.Status);
            }

            if (exam.Data!.StartDate > DateTimeOffset.UtcNow)
            {
                return ServiceResult.Fail("Exam has not started yet.", HttpStatusCode.BadRequest);
            }

            var examEndTime = examResult.Data!.StartDate.AddMinutes(exam.Data.Duration);
            var finalExamEndTime = exam.Data.EndDate < examEndTime ? exam.Data.EndDate : examEndTime;
            if (DateTimeOffset.UtcNow > finalExamEndTime)
            {
                return ServiceResult.Fail("Exam has already ended", HttpStatusCode.BadRequest);
            }

            //Cevap doğru ise true, yanlış ise false olacak şekilde kontrol edilir
            var isCorrect = !string.IsNullOrEmpty(request.SelectedAnswer) &&
                            request.SelectedAnswer.Equals(question.Data!.CorrectAnswer, StringComparison.OrdinalIgnoreCase);
            
            mapper.Map(request, answer);
            answer.IsCorrect = isCorrect;

            answerRepository.Update(answer);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<AnswerResponseDto?>> GetByIdAsync(int id)
        {
            var answer = await answerRepository.GetByIdAsync(id);

            if (answer is null)
            {
                return ServiceResult<AnswerResponseDto>.Fail("Answer not found", HttpStatusCode.NotFound)!;
            }
            answer.CreatedDate = dateTimeService.ConvertFromUtc(answer.CreatedDate);
            var answerAsDto = mapper.Map<AnswerResponseDto>(answer);
            answer.CreatedDate = dateTimeService.ConvertToUtc(answer.CreatedDate);

            return ServiceResult<AnswerResponseDto>.Success(answerAsDto)!;
        }

        public async Task<ServiceResult> DeleteAsync(int id, int userId)
        {
            var answer = await answerRepository.GetByIdAsync(id);
            if (answer is null)
            {
                return ServiceResult.Fail("Answer not found", HttpStatusCode.NotFound);
            }

            if (answer.UserId != userId)
            {
                return ServiceResult.Fail("You are not authorized to delete this answer.", HttpStatusCode.Unauthorized);
            }

            answerRepository.Delete(answer);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<List<AnswerResponseDto>>> GetByUserAndExamAsync(int userId, int examId)
        {
            var answers = await answerRepository.GetByUserAndExamAsync(userId, examId);
            if (answers.Count == 0)
            {
                return ServiceResult<List<AnswerResponseDto>>.Fail("Answers not found.", HttpStatusCode.NotFound);
            }

            var answerAsDto = answers.Select(a =>
            {
                a.CreatedDate = dateTimeService.ConvertFromUtc(a.CreatedDate);
                var dto = mapper.Map<AnswerResponseDto>(a);
                a.CreatedDate = dateTimeService.ConvertToUtc(a.CreatedDate);
                return dto;
            }).ToList();
            return ServiceResult<List<AnswerResponseDto>>.Success(answerAsDto);
        }

    }
}
