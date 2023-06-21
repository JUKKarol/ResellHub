using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellHub.Services.UserServices;
using System.Security.Claims;

namespace ResellHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IUserService _userService;

        public ChatController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserChats(Guid userId)
        {
            if (!await _userService.CheckIsUserExistById(userId))
            {
                return BadRequest("user doesn't exist");
            }

            return Ok(await _userService.GetUserChats(userId));
        }

        [HttpPost("{fromUserId}/{toUserId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> CreateNewChat(Guid fromUserId, Guid toUserId)
        {
            if (await _userService.CheckIsChatExistsByUsersId(fromUserId, toUserId))
            {
                return BadRequest("Chat already exist");
            }

            var loggedUserId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (fromUserId != loggedUserId && toUserId != loggedUserId)
            {
                return BadRequest("permission dennied");
            }

            return Ok(await _userService.CreateChat(fromUserId, toUserId));
        }
    }
}
