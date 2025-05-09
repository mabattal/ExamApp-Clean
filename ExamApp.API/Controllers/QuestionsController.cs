using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ExamApp.Application.Features.Questions;
using ExamApp.Application.Features.Questions.Create;
using ExamApp.Application.Features.Questions.Update;

namespace ExamApp.API.Controllers
{
    [Authorize]
    public class QuestionsController(IQuestionService questionService) : CustomBaseController
    {
        [Authorize(Roles = "Student, Instructor, Admin")]
        [HttpGet("{questionId:int}")]
        public async Task<IActionResult> GetById(int questionId)
        {
            var result = await questionService.GetByIdAsync(questionId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Student, Instructor, Admin")]
        [HttpGet("examId/{examId:int}")]
        public async Task<IActionResult> GetByExamId(int examId)
        {
            var result = await questionService.GetByExamIdAsync(examId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpGet("withCorrectAnswer/examId/{examId:int}")]
        public async Task<IActionResult> GetByExamIdWithCorrectAnswer(int examId)
        {
            var result = await questionService.GetByExamIdWithCorrectAnswerAsync(examId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(CreateQuestionRequestDto createQuestionRequest)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await questionService.AddAsync(createQuestionRequest, userId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpPut("{questionId:int}")]
        public async Task<IActionResult> Update(int questionId, UpdateQuestionRequestDto updateQuestionRequest)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await questionService.UpdateAsync(questionId, updateQuestionRequest, userId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpDelete("{questionId:int}")]
        public async Task<IActionResult> Delete(int questionId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await questionService.DeleteAsync(questionId, userId);
            return CreateActionResult(result);
        }

    }
}
