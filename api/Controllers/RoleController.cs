using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellHub.Entities;
using ResellHub.Enums;
using ResellHub.Services.UserServices;

namespace ResellHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUserService _userService;

        public RoleController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}"), Authorize(Roles = "Moderator")]
        public async Task<IActionResult> GetUserRoles(Guid userId)
        {
            if (!await _userService.CheckIsUserExistById(userId))
            {
                return BadRequest("user doesn't exist");
            }

            return Ok(await _userService.GetUserRoles(userId));
        }

        [HttpPost("{userId}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateRole(Guid userId, UserRoles userRole)
        {
            if (!await _userService.CheckIsUserExistById(userId))
            {
                return BadRequest("user doesn't exist");
            }

            return Ok(await _userService.AddRole(userId, userRole));
        }

        [HttpPut("{roleId}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ChangeRole(Guid roleId, UserRoles userNewRole)
        {
            if (!await _userService.CheckIsRoleExistById(roleId))
            {
                return BadRequest("role doesn't exist");
            }

            return Ok(await _userService.UpdateRole(roleId, userNewRole));
        }

        [HttpDelete("{roleId}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            if (!await _userService.CheckIsRoleExistById(roleId))
            {
                return BadRequest("role doesn't exist");
            }

            return Ok(await _userService.DeleteRole(roleId));
        }
    }
}
