using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ExamApp.Application.Features.Answers;
using ExamApp.Application.Features.Answers.Create;
using ExamApp.Application.Features.Answers.Update;

namespace ExamApp.API.Controllers
{
    [Authorize]
    public class AnswersController(IAnswerService answerService) : CustomBaseController
    {
        [Authorize(Roles = "Student, Instructor, Admin")]
        [HttpGet("{answerId:int}")]
        public async Task<IActionResult> GetById(int answerId)
        {
            var result = await answerService.GetByIdAsync(answerId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        public async Task<IActionResult> Add(CreateAnswerRequestDto createAnswerRequest)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await answerService.AddAsync(createAnswerRequest, userId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Student")]
        [HttpPut("{answerId:int}")]
        public async Task<IActionResult> Update(int answerId, UpdateAnswerRequestDto updateAnswerRequest)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await answerService.UpdateAsync(answerId, updateAnswerRequest, userId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Student")]
        [HttpDelete("{answerId:int}")]
        public async Task<IActionResult> Delete(int answerId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await answerService.DeleteAsync(answerId, userId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Student, Instructor, Admin")]
        [HttpGet("exam/{examId:int}")]
        public async Task<IActionResult> GetByUserAndExam(int examId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await answerService.GetByUserAndExamAsync(userId, examId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpGet("exam/{userId:int}/{examId:int}")]
        public async Task<IActionResult> GetByUserAndExam(int userId, int examId)
        {
            var result = await answerService.GetByUserAndExamAsync(userId, examId);
            return CreateActionResult(result);
        }

    }
}
