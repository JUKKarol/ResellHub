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
    }
}
