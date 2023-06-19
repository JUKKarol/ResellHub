using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellHub.DTOs.OfferDTOs;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Services.OfferServices;
using ResellHub.Services.UserServices;
using System.Security.Claims;

namespace ResellHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;
        private readonly IUserService _userService;

        public OfferController(IOfferService offerService, IUserService userService)
        {
            _offerService = offerService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOffers()
        {
            return Ok(await _offerService.GetOffers());
        }

        [HttpGet("{userId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserOffers(Guid userId)
        {
            if (!await _userService.CheckIsUserExistById(userId))
            {
                return BadRequest("user doesn't exist");
            }

            return Ok(await _offerService.GetUserOffers(userId));
        }

        [HttpPost, Authorize(Roles = "User")]
        public async Task<IActionResult> CreateOffer(OfferCreateDto offerDto)
        {
            return Ok(await _offerService.AddOffer(offerDto, HttpContext.User.FindFirstValue(ClaimTypes.Email)));
        }

        [HttpPut("{offerId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateOffer(Guid offerId, OfferCreateDto offerDto)
        {
            if (!await _offerService.CheckIsOfferExistById(offerId))
            {
                return BadRequest("offer doesn't exist");
            }

            if (!await _offerService.CheckIsOfferOwnerCorrectByEmail(offerId, HttpContext.User.FindFirstValue(ClaimTypes.Email)))
            {
                return BadRequest("you aren't offer owner");
            }

            return Ok(await _offerService.UpdateOffer(offerId, offerDto));
        }

        [HttpDelete("{offerId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteOffer(Guid offerId)
        {
            if (!await _offerService.CheckIsOfferExistById(offerId))
            {
                return BadRequest("offer doesn't exist");
            }

            if (!await _offerService.CheckIsOfferOwnerCorrectByEmail(offerId, HttpContext.User.FindFirstValue(ClaimTypes.Email)))
            {
                return BadRequest("you aren't offer owner");
            }

            return Ok(await _offerService.DeleteOffer(offerId));
        }
    }
}
