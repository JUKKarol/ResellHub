using FluentValidation;
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
        private readonly IValidator<OfferCreateDto> _offerCreateValidator;

        public OfferController(IOfferService offerService, IUserService userService, IValidator<OfferCreateDto> offerCreateValidator)
        {
            _offerService = offerService;
            _userService = userService;
            _offerCreateValidator = offerCreateValidator;
        }

        [HttpGet, Authorize(Roles = "User"), AllowAnonymous]
        public async Task<IActionResult> GetOffers(int page = 1)
        {
            Guid loggedUserId = User.Identity.IsAuthenticated ? Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty;
            var offers = await _offerService.GetOffers(page, loggedUserId);

            return Ok(offers);
        }

        [HttpGet("{offerSlug}"), Authorize(Roles = "User"), AllowAnonymous]
        public async Task<IActionResult> GetOfferBySlug(string offerSlug)
        {
            Guid loggedUserId = User.Identity.IsAuthenticated ? Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty;
            var offer = await _offerService.GetOfferBySlug(offerSlug, loggedUserId);

            if (offer == null)
            {
                return NotFound("offer doesn't exist");
            }

            return Ok(offer);
        }

        [HttpPost, Authorize(Roles = "User")]
        public async Task<IActionResult> CreateOffer(OfferCreateDto offerDto)
        {
            var validationResult = await _offerCreateValidator.ValidateAsync(offerDto);
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(error => error.ErrorMessage);
                return BadRequest(string.Join(Environment.NewLine, validationErrors));
            }

            if (!await _offerService.AddOffer(offerDto, Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))))
            {
                return BadRequest("error while creating");
            }
            return Ok("offer created");
        }

        [HttpPut("{offerId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateOffer(Guid offerId, OfferCreateDto offerDto)
        {
            var validationResult = await _offerCreateValidator.ValidateAsync(offerDto);
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(error => error.ErrorMessage);
                return BadRequest(string.Join(Environment.NewLine, validationErrors));
            }

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
