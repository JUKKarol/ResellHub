using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Services.UserServices;

namespace ResellHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto userDto)
        {
            return Ok(await _userService.CreateUser(userDto));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginOwner(UserLoginDto userDto)
        {
            return Ok(await _userService.LoginUser(userDto));
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            return Ok(await _userService.VerifyUser(token));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string userEmail)
        {
            return Ok(await _userService.ForgotPassword(userEmail));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(UserResetPasswordDto userDto)
        {
            return Ok(await _userService.ResetPassword(userDto));
        }
    }
}
