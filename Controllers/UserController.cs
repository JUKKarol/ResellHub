using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Enums;
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

        //User CRUD
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

        //Message

        [HttpGet("messages/{firstUser}/{secondUser}")]
        public async Task<IActionResult> GetUsersMessages(Guid firstUser, Guid secondUser)
        {
            var messages = await _userService.ShowUsersMessages(firstUser, secondUser);

            return Ok(messages);
        }

        [HttpPost("messages/{fromUserId}/{toUserId}")]
        public async Task<IActionResult> SendMessage(Guid fromUserId, Guid toUserId, string content)
        {
            var actionInfo = await _userService.SendMessage(fromUserId, toUserId, content);

            return Ok(actionInfo);
        }

        //Role
        [HttpGet("roles/{userId}")]
        public async Task<IActionResult> GetUserRoles(Guid userId)
        {
            var actionInfo = await _userService.GetUserRoles(userId);

            return Ok(actionInfo);
        }

        [HttpPost("roles/{userId}")]
        public async Task<IActionResult> CreateRole(Guid userId, UserRoles userRole)
        {
            var actionInfo = await _userService.AddRole(userId, userRole);

            return Ok(actionInfo);
        }

        [HttpPut("roles/{roleId}")]
        public async Task<IActionResult> ChangeRole(Guid roleId, UserRoles userNewRole)
        {
            var actionInfo = await _userService.UpdateRole(roleId, userNewRole);

            return Ok(actionInfo);
        }

        [HttpDelete("roles/{roleId}")]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            var actionInfo = await _userService.DeleteRole(roleId);

            return Ok(actionInfo);
        }


        //FollowOffer
    }
}
