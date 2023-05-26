using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellHub.Services.UserServices;

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

        [HttpGet("messages/{firstUserId}/{secondUserId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetUsersMessages(Guid firstUserId, Guid secondUserId)
        {
            if (!await _userService.CheckIsUserExistById(firstUserId) || !await _userService.CheckIsUserExistById(secondUserId))
            {
                return BadRequest("sender or receiver doesn't exist");
            }

            var messages = await _userService.ShowUsersMessages(firstUserId, secondUserId);

            return Ok(messages);
        }

        [HttpPost("messages/{fromUserId}/{toUserId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> SendMessage(Guid fromUserId, Guid toUserId, string content)
        {
            if (!await _userService.CheckIsUserExistById(fromUserId) || !await _userService.CheckIsUserExistById(toUserId))
            {
                return BadRequest("sender or receiver doesn't exist");
            }

            return Ok(await _userService.SendMessage(fromUserId, toUserId, content));
        }
    }
}
