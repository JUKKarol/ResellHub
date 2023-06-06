using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Enums;
using ResellHub.Services.UserServices;
using System.Security.Claims;

namespace ResellHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }

        [HttpGet("users/{userId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            if (!await _userService.CheckIsUserExistById(userId))
            {
                return BadRequest("user doesn't exist");
            }

            return Ok(await _userService.GetUserById(userId));
        }

        [HttpPost("users"), Authorize(Roles = "Moderator")]
        public async Task<IActionResult> CreateUser(UserRegistrationDto userDto)
        {
            if (!await _userService.CheckIsUserExistByEmail(userDto.Email))
            {
                return BadRequest("email is already in use");
            }

            return Ok(_userService.CreateUser(userDto));
        }

        [HttpPut("users/{userId}/phonenumber"), Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateUserPhoneNumber(string newPhoneNumber)
        {
            var userId = await _userService.GetUserIdByEmail(HttpContext.User.FindFirstValue(ClaimTypes.Email));

            if (!await _userService.CheckIsUserExistById(userId))
            {
                return BadRequest("user doesn't exist");
            }

            return Ok(await _userService.UpdatePhoneNumber(userId, newPhoneNumber));
        }

        [HttpPut("users/{userId}/city"), Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateUserCity(string newCity)
        {
            var userId = await _userService.GetUserIdByEmail(HttpContext.User.FindFirstValue(ClaimTypes.Email));

            if (!await _userService.CheckIsUserExistById(userId))
            {
                return BadRequest("user doesn't exist");
            }

            return Ok(await _userService.UpdateCity(userId, newCity));
        }

        [HttpDelete("users/{userId}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            if (!await _userService.CheckIsUserExistById(userId))
            {
                return BadRequest("user doesn't exist");
            }

            return Ok(await _userService.DeleteUser(userId));
        }

        [HttpDelete("users"), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteAccount()
        {
            return Ok(await _userService.DeleteUser(await _userService.GetUserIdByEmail(HttpContext.User.FindFirstValue(ClaimTypes.Email))));
        }
    }
}
