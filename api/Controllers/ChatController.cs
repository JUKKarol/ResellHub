using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellHub.DTOs.ChatDTOs;
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

        [HttpGet, Authorize(Roles = "User")]
        public async Task<IActionResult> GetMyChats(int page = 1)
        {
            if (page <= 0)
            {
                return BadRequest("page must be greater than 0");
            }

            return Ok(await _userService.GetUserChats(Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)), page));
        }

        [HttpPost, Authorize(Roles = "User")]
        public async Task<IActionResult> CreateNewChat([FromBody] ChatCreateDto chatDto)
        {
            if (await _userService.CheckIsChatExistsByUsersId(chatDto.SenderId, chatDto.ReciverId))
            {
                return BadRequest("Chat already exist");
            }

            var loggedUserId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (chatDto.SenderId != loggedUserId && chatDto.ReciverId != loggedUserId)
            {
                return BadRequest("permission denied");
            }

            return Ok(await _userService.CreateChat(chatDto.SenderId, chatDto.ReciverId));
        }
    }
}
