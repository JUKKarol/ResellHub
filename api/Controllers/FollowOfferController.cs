﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{userId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetMyFollowingOffers(int page = 1)
        {
            return Ok(await _userService.GetUserFollowingOffers(await _userService.GetUserIdByEmail(HttpContext.User.FindFirstValue(ClaimTypes.Email)), page));
        }

        [HttpPost("{offerId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> AddOfferToFollowing(Guid offerId)
        {
            return Ok(await _userService.AddOfferToFollowing(await _userService.GetUserIdByEmail(HttpContext.User.FindFirstValue(ClaimTypes.Email)), offerId));
        }

        [HttpDelete("{offerId}"), Authorize(Roles = "User")]
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
