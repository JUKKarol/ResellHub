using Microsoft.AspNetCore.Mvc;
using ResellHub.DTOs.UserDTOs;
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

            if (users.Count == 0)
            {
                return NotFound("User didn't exist");
            }

            return Ok(users);
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user == null)
            {
                return NotFound("User didn't exist");
            }

            return Ok(user);
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser(UserRegistrationDto userDto)
        {
            var actionInfo = await _userService.CreateUser(userDto);

            return Ok(actionInfo);
        }

        [HttpPut("users/{userId}/phonenumber")]
        public async Task<IActionResult> UpdateUserPhoneNumber(Guid userId, string newPhoneNumber)
        {
            var actionInfo = await _userService.UpdatePhoneNumber(userId, newPhoneNumber);

            return Ok(actionInfo);
        }

        [HttpPut("users/{userId}/city")]
        public async Task<IActionResult> UpdateUserCity(Guid userId, string newCity)
        {
            var actionInfo = await _userService.UpdateCity(userId, newCity);

            return Ok(actionInfo);
        }

        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var actionInfo = await _userService.DeleteUser(userId);

            return Ok(actionInfo);
        }
    }
}
