using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet(), Authorize(Roles = "User")]
        public async Task<IActionResult> GetMyFollowingOffers(int page = 1)
        {
            return Ok(await _userService.GetUserFollowingOffers(Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)), page));
        }

        [HttpPost("{offerSlug}"), Authorize(Roles = "User")]
        public async Task<IActionResult> AddOfferToFollowing(string offerSlug)
        {
            var offerId = await _offerService.GetOfferIdBySlug(offerSlug);

            if (offerId == Guid.Empty)
            {
                return NotFound("offer not found");
            }

            await _userService.AddOfferToFollowing(Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)), offerId);

            return Ok("Offer is following from now");
        }

        [HttpDelete("{offerslug}"), Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveOfferFromFollowing(string offerSlug)
        {
            var offerId = await _offerService.GetOfferIdBySlug(offerSlug);

            if (!await _offerService.CheckIsOfferExistById(offerId))
            {
                return BadRequest("following offer doesn't exist");
            }

            var followingOfferId = 
                await _userService.GetFollowingOfferByUserAndOfferId(await _userService.GetUserIdByEmail(HttpContext.User.FindFirstValue(ClaimTypes.Email)), offerId);

            await _userService.RemoveOfferFromFollowing(followingOfferId.Id);

            return Ok("Offer is not following anymore");
        }
    }
}
