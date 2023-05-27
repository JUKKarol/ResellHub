using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellHub.Entities;
using ResellHub.Services.OfferServices;
using ResellHub.Services.UserServices;

namespace ResellHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowOfferController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOfferService _offerService;

        public FollowOfferController(IUserService userService, IOfferService offerService)
        {
            _userService = userService;
            _offerService = offerService;
        }

        [HttpGet("followingoffers/{userId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserFollowingOffers(Guid userId)
        {
            if (!await _userService.CheckIsUserExistById(userId))
            {
                return BadRequest("user doesn't exist");
            }

            await _userService.GetUserFollowingOffers(userId);

            return Ok(await _userService.GetUserFollowingOffers(userId));
        }

        [HttpPost("followingoffers/{userId}/{offerId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> AddOfferToFollowing(Guid userId, Guid offerId)
        {
            if (!await _userService.CheckIsUserExistById(userId) || !await _offerService.CheckIsOfferExistById(offerId))
            {
                return BadRequest("user or offer doesn't exist");
            }

            return Ok(await _userService.AddOfferToFollowing(userId, offerId));
        }

        [HttpDelete("followingoffers/{followingOfferId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveOfferFromFollowing(Guid followingOfferId)
        {
            if (!await _userService.CheckIsFollowingExistById(followingOfferId))
            {
                return BadRequest("following offer doesn't exist");
            }

            return Ok(await _userService.RemoveOfferFromFollowing(followingOfferId));
        }
    }
}
