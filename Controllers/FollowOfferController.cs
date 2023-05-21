using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellHub.Services.UserServices;

namespace ResellHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowOfferController : ControllerBase
    {
        private readonly IUserService _userService;

        public FollowOfferController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("followingoffers/{userId}")]
        public async Task<IActionResult> GetUserFollowingOffers(Guid userId)
        {
            var actionInfo = await _userService.GetUserFollowingOffers(userId);

            return Ok(actionInfo);
        }

        [HttpPost("followingoffers/{userId}/{offerId}")]
        public async Task<IActionResult> AddOfferToFollowing(Guid userId, Guid offerId)
        {
            var actionInfo = await _userService.AddOfferToFollowing(userId, offerId);

            return Ok(actionInfo);
        }

        [HttpDelete("followingoffers/{followingOfferId}")]
        public async Task<IActionResult> RemoveOfferFromFollowing(Guid followingOfferId)
        {
            var actionInfo = await _userService.RemoveOfferFromFollowing(followingOfferId);

            return Ok(actionInfo);
        }
    }
}
