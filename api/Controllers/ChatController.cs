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

        [HttpGet("{userId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserChats(Guid userId, int page = 1)
        {
            if (page <= 0)
            {
                return BadRequest("page must be greater than 0");
            }

            if (!await _userService.CheckIsUserExistById(userId))
            {
                return BadRequest("user doesn't exist");
            }

            if (Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) != userId)
            { 
                return BadRequest("permission dennied");
            }

            return Ok(await _userService.GetUserChats(userId, page));
        }

        [HttpPost, Authorize(Roles = "User")]
        public async Task<IActionResult> CreateNewChat([FromBody] ChatCreateDto chatDto)
        {
            if (await _userService.CheckIsChatExistsByUsersId(chatDto.FromUserId, chatDto.ToUserId))
            {
                return BadRequest("Chat already exist");
            }

            var loggedUserId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (chatDto.FromUserId != loggedUserId && chatDto.ToUserId != loggedUserId)
            {
                return BadRequest("permission dennied");
            }

            return Ok(await _userService.CreateChat(chatDto.FromUserId, chatDto.ToUserId));
        }
    }
}
