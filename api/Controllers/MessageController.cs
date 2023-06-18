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

        [HttpGet("{chatId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetMessagesByChatId(Guid chatId)
        {
            var chat = await _userService.GetChatById(chatId);

            if (chat == null)
            {
                return BadRequest("chat doesn't exist");
            }

            var loggedUserId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (chat.ToUserId != loggedUserId && chat.FromUserId != loggedUserId)
            { 
                return BadRequest("permission dennied");
            }

            var messages = await _userService.GetMessagesByChatId(chatId);

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
