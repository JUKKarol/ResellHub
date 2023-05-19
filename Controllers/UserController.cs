using Microsoft.AspNetCore.Authorization;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();

            return Ok(users);
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser(UserRegistrationDto userDto)
        {
            var actionInfo = await _userService.CreateUser(userDto);

            return Ok(actionInfo);
        }
    }
}
