using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
