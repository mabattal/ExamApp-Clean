using System.Net;
using AutoMapper;
using ExamApp.Application.Contracts.Persistence;
using ExamApp.Application.Features.Exams;
using ExamApp.Application.Features.Questions.Create;
using ExamApp.Application.Features.Questions.Dto;
using ExamApp.Application.Features.Questions.Update;
using ExamApp.Domain.Entities;

namespace ExamApp.Application.Features.Questions
{
    public class QuestionService(
        IQuestionRepository questionRepository,
        IExamService examService,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IQuestionService
    {
        public async Task<ServiceResult<QuestionResponseDto?>> GetByIdAsync(int id)
        {
            var question = await questionRepository.GetByIdAsync(id);
            if (question is null)
            {
                return ServiceResult<QuestionResponseDto>.Fail("Question not found", HttpStatusCode.NotFound)!;
            }

            var exam = await examService.GetByIdAsync(question.ExamId);
            if (exam.IsFail)
            {
                return ServiceResult<QuestionResponseDto>.Fail("The exam of the question has been deleted", HttpStatusCode.BadRequest)!;
            }

            var questionAsDto = mapper.Map<QuestionResponseDto>(question);

            return ServiceResult<QuestionResponseDto>.Success(questionAsDto)!;
        }

        public async Task<ServiceResult<CreateQuestionResponseDto>> AddAsync(CreateQuestionRequestDto createQuestionRequest, int userId)
        {
            var exam = await examService.GetByIdAsync(createQuestionRequest.ExamId);
            if (exam.IsFail)
            {
                return ServiceResult<CreateQuestionResponseDto>.Fail(exam.ErrorMessage!, exam.Status);
            }

            if (exam.Data!.Instructor.UserId != userId)
            {
                return ServiceResult<CreateQuestionResponseDto>.Fail("You are not authorized to add a question to this exam.", HttpStatusCode.Unauthorized)!;
            }

            if (DateTimeOffset.UtcNow > exam.Data!.StartDate)
            {
                return ServiceResult<CreateQuestionResponseDto>.Fail("You cannot add a question to an exam that has already started.", HttpStatusCode.BadRequest)!;
            }

            var questionResult = await questionRepository.AnyAsync(x => x.ExamId == createQuestionRequest.ExamId && x.QuestionText == createQuestionRequest.QuestionText);
            if (questionResult)
            {
                return ServiceResult<CreateQuestionResponseDto>.Fail("Question already exists in this exam.", HttpStatusCode.BadRequest)!;
            }

            var question = mapper.Map<Question>(createQuestionRequest);

            await questionRepository.AddAsync(question);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult<CreateQuestionResponseDto>.Success(new CreateQuestionResponseDto(question.QuestionId));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateQuestionRequestDto updateQuestionRequest, int userId)
        {
            var question = await questionRepository.GetByIdWithExamAsync(id);
            if (question is null)
            {
                return ServiceResult.Fail("Question not found", HttpStatusCode.NotFound);
            }

            if (question.Exam.CreatedBy != userId)
            {
                return ServiceResult.Fail("You are not authorized to update this question.", HttpStatusCode.Unauthorized);
            }

            if (DateTimeOffset.UtcNow > question.Exam.StartDate)
            {
                return ServiceResult.Fail("You cannot update a question in an exam that has already started.", HttpStatusCode.BadRequest);
            }

            //question.QuestionText = updateQuestionRequest.QuestionText;
            //question.OptionA = updateQuestionRequest.OptionA;
            //question.OptionB = updateQuestionRequest.OptionB;
            //question.OptionC = updateQuestionRequest.OptionC;
            //question.OptionD = updateQuestionRequest.OptionD;
            //question.CorrectAnswer = updateQuestionRequest.CorrectAnswer;

            question = mapper.Map(updateQuestionRequest, question);

            questionRepository.Update(question);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id, int userId)
        {
            var question = await questionRepository.GetByIdWithExamAsync(id);
            if (question is null)
            {
                return ServiceResult.Fail("Question not found", HttpStatusCode.NotFound);
            }

            if (question.Exam.CreatedBy != userId)
            {
                return ServiceResult.Fail("You are not authorized to update this question.", HttpStatusCode.Unauthorized);
            }

            if (DateTimeOffset.UtcNow > question.Exam.StartDate)
            {
                return ServiceResult.Fail("You cannot delete a question in an exam that has already started.", HttpStatusCode.BadRequest);
            }
            questionRepository.Delete(question);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<List<QuestionResponseWithoutCorrectAnswerDto>>> GetByExamIdAsync(int examId)
        {
            var exam = await examService.GetByIdAsync(examId);
            if (exam.IsFail)
            {
                return ServiceResult<List<QuestionResponseWithoutCorrectAnswerDto>>.Fail(exam.ErrorMessage!, exam.Status);
            }

            var questions = await questionRepository.GetByExamIdAsync(examId);
            if (!questions.Any())
            {
                return ServiceResult<List<QuestionResponseWithoutCorrectAnswerDto>>.Fail("No questions found for the given exam.", HttpStatusCode.NotFound);
            }

            var questionsAsDto = mapper.Map<List<QuestionResponseWithoutCorrectAnswerDto>>(questions);
            return ServiceResult<List<QuestionResponseWithoutCorrectAnswerDto>>.Success(questionsAsDto);
        }

        public async Task<ServiceResult<List<QuestionResponseDto>>> GetByExamIdWithCorrectAnswerAsync(int examId)
        {
            var exam = await examService.GetByIdAsync(examId);
            if (exam.IsFail)
            {
                return ServiceResult<List<QuestionResponseDto>>.Fail(exam.ErrorMessage!, exam.Status);
            }

            var questions = await questionRepository.GetByExamIdAsync(examId);
            if (!questions.Any())
            {
                return ServiceResult<List<QuestionResponseDto>>.Fail("No questions found for the given exam.", HttpStatusCode.NotFound);
            }

            var questionsAsDto = mapper.Map<List<QuestionResponseDto>>(questions);
            return ServiceResult<List<QuestionResponseDto>>.Success(questionsAsDto);
        }
    }
}
