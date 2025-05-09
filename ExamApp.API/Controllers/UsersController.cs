using ExamApp.Application.Features.Users;
using ExamApp.Application.Features.Users.Create;
using ExamApp.Application.Features.Users.Update;
using ExamApp.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers
{
    [Authorize(Roles = "Instructor, Admin, Student")]
    public class UsersController(IUserService userService) : CustomBaseController
    {
        //kısa yazımı
        //[HttpGet]
        //public async Task<IActionResult> GetAll() => CreateActionResult(await _userService.GetAllAsync());

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await userService.GetAllAsync();
            return CreateActionResult(result);
        }

        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetPagedAll(int pageNumber, int pageSize)
        {
            var result = await userService.GetPagedAllAsync(pageNumber, pageSize);
            return CreateActionResult(result);
        }

        [HttpGet("{value}")]
        public async Task<IActionResult> GetByIdOrEmail(string value)
        {
            var result = await userService.GetByIdOrEmailAsync(value);
            return CreateActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateUserRequestDto createUserRequest)
        {
            var result = await userService.AddAsync(createUserRequest);
            return CreateActionResult(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateUserRequestDto updateUserRequest)
        {
            var result = await userService.UpdateAsync(id, updateUserRequest);
            return CreateActionResult(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await userService.DeleteAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("role/{role:int}")]
        public async Task<IActionResult> GetByRole(UserRole role)
        {
            var result = await userService.GetByRole(role);
            return CreateActionResult(result);
        }
    }
}
