﻿using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetMessagesByChatId(Guid chatId, [FromQuery] int page = 1)
        {
            if (page <= 0)
            {
                return BadRequest("page must be greater than 0");
            }

            var chat = await _userService.GetChatById(chatId);

            if (chat == null)
            {
                return BadRequest("chat doesn't exist");
            }

            var loggedUserId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (chat.ReciverId != loggedUserId && chat.SenderId != loggedUserId)
            {
                return BadRequest("permission denied");
            }

            var messages = await _userService.GetMessagesByChatId(chatId, page);

            return Ok(messages);
        }

        [HttpPost("{chatId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> SendMessage(Guid chatId, string content)
        {
            var chat = await _userService.GetChatById(chatId);

            if (chat == null)
            {
                return BadRequest("chat doesn't exist");
            }

            var loggedUserId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (chat.SenderId != loggedUserId)
            {
                return BadRequest("permission denied");
            }

            return Ok(await _userService.SendMessage(chatId, loggedUserId, content));
        }
    }
}