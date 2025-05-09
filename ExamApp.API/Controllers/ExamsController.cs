using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ExamApp.Application.Features.Exams;
using ExamApp.Application.Features.Exams.Create;
using ExamApp.Application.Features.Exams.Update;

namespace ExamApp.API.Controllers
{
    [Authorize]
    public class ExamsController(IExamService examService) : CustomBaseController
    {
        [Authorize(Roles = "Instructor, Admin")]
        [HttpGet("instructor/{instructorId:int}")]
        public async Task<IActionResult> GetByInstructor()
        {
            var instructorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await examService.GetByInstructorAsync(instructorId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Student, Instructor, Admin")]
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveExams()
        {
            var result = await examService.GetActiveExamsAsync();
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Student, Instructor, Admin")]
        [HttpGet("past")]
        public async Task<IActionResult> GetPastExams()
        {
            var result = await examService.GetPastExamsAsync();
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Student, Instructor, Admin")]
        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingExams()
        {
            var result = await examService.GetUpcomingExamsAsync();
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Student, Instructor, Admin")]
        [HttpGet("{examId:int}")]
        public async Task<IActionResult> GetById(int examId)
        {
            var result = await examService.GetByIdAsync(examId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(CreateExamRequestDto createExamRequest)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await examService.AddAsync(createExamRequest, userId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpPut("{examId:int}")]
        public async Task<IActionResult> Update(int examId, UpdateExamRequestDto updateExamRequest)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await examService.UpdateAsync(examId, updateExamRequest, userId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpDelete("{examId:int}")]
        public async Task<IActionResult> Delete(int examId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await examService.DeleteAsync(examId, userId);
            return CreateActionResult(result);
        }
    }
}
