﻿using ExamApp.Application.Contracts.Authentication;
using ExamApp.Application.Contracts.Authentication.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers
{
    public class AuthController(IAuthService authService) : CustomBaseController
    {
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var result = await authService.ValidateUserAsync(loginRequest.Email, loginRequest.Password);
            return CreateActionResult(result);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request)
        {
            var result = await authService.RegisterAsync(request);
            return CreateActionResult(result);
        }

        [Authorize]
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            //kullanıcıyı logout yapmak için client tarafında token'ı sıfırlamak yeterli olacaktır
            return Ok(new { message = "Successfully logged out" });
        }
    }
}