using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellHub.Services.UserServices;
using System.Security.Claims;

namespace ResellHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IUserService _userService;

        public MessageController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{firstUserId}/{secondUserId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetUsersMessages(Guid firstUserId, Guid secondUserId)
        {
            if (!await _userService.CheckIsUserExistById(firstUserId) || !await _userService.CheckIsUserExistById(secondUserId))
            {
                return BadRequest("sender or receiver doesn't exist");
            }

            var loggedUserEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (await _userService.GetUserEmailById(firstUserId) != loggedUserEmail && await _userService.GetUserEmailById(secondUserId) != loggedUserEmail)
            {
                return BadRequest("sender or receiver aren't logged");
            }

            var messages = await _userService.ShowUsersMessages(firstUserId, secondUserId);

            return Ok(messages);
        }

        [HttpPost("{fromUserId}/{toUserId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> SendMessage(Guid fromUserId, Guid toUserId, string content)
        {
            if (!await _userService.CheckIsUserExistById(fromUserId) || !await _userService.CheckIsUserExistById(toUserId))
            {
                return BadRequest("sender or receiver doesn't exist");
            }

            if (await _userService.GetUserEmailById(fromUserId) != HttpContext.User.FindFirstValue(ClaimTypes.Email))
            {
                return BadRequest("sender isn't logged");
            }

            return Ok(await _userService.SendMessage(fromUserId, toUserId, content));
        }
    }
}
