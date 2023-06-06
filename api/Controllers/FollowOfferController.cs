using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellHub.Entities;
using ResellHub.Services.OfferServices;
using ResellHub.Services.UserServices;
using System.Security.Claims;

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
        public async Task<IActionResult> GetMyFollowingOffers()
        {
            return Ok(await _userService.GetUserFollowingOffers(await _userService.GetUserIdByEmail(HttpContext.User.FindFirstValue(ClaimTypes.Email))));
        }

        [HttpPost("followingoffers/{offerId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> AddOfferToFollowing(Guid offerId)
        {
            return Ok(await _userService.AddOfferToFollowing(await _userService.GetUserIdByEmail(HttpContext.User.FindFirstValue(ClaimTypes.Email)), offerId));
        }

        [HttpDelete("followingoffers/{offerId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveOfferFromFollowing(Guid offerId)
        {
            if (!await _offerService.CheckIsOfferExistById(offerId))
            {
                return BadRequest("following offer doesn't exist");
            }

            var followingOfferId = 
                await _userService.GetFollowingOfferByUserAndOfferId(await _userService.GetUserIdByEmail(HttpContext.User.FindFirstValue(ClaimTypes.Email)), offerId);

            return Ok(await _userService.RemoveOfferFromFollowing(followingOfferId.Id));
        }
    }
}
