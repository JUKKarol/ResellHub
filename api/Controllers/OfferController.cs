using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResellHub.DTOs.OfferDTOs;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Services.FileServices;
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
        private readonly IFileService _fileService;

        public OfferController(IOfferService offerService, IUserService userService, IFileService fileService)
        {
            _offerService = offerService;
            _userService = userService;
            _fileService = fileService;
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

            return Ok(await _offerService.GetOfferBySlug(offerSlug, loggedUserId));
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

        //images
        [HttpGet("image/{offerslug}"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetOfferImages(string offerslug)
        {
            var offer = await _offerService.GetOfferBySlug(offerslug, Guid.Empty);

            if (offer == null)
            {
                return NotFound("offer didn't exist");
            }

            if (!offer.OfferImages.Any())
            {
                return NotFound("offer didn't have uploaded images yet");
            }

            var offerImages = await _fileService.GetOfferImagesByOfferSlug(offerslug);

            foreach (var offerImage in offerImages)
            {
                if (offerImage.ImageBytes.Length < 1)
                {
                    return BadRequest("error while uploading file");
                }
            }

            return Ok(offerImages);
        }

        [HttpPost("image/{offerSlug}"), Authorize(Roles = "User")]
        public async Task<IActionResult> UploadOfferImage(IFormFile image, string offerSlug)
        {
            var userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var offer = await _offerService.GetOfferBySlug(offerSlug, Guid.Empty);

            if (offer.OfferImages.Count > 3)
            { 
                return BadRequest("offer images are full");
            }

            if (offer.UserId != userId)
            { 
                return BadRequest("permission denied");
            }

            if (image == null)
            {
                return BadRequest("image can't be empty");
            }

            if (!_fileService.CheckIsOfferImageSizeCorrect(image))
            {
                return BadRequest("image is to large");
            }

            if (!await _fileService.AddOfferImage(image, await _offerService.GetOfferIdByOfferSlug(offerSlug)))
            {
                return BadRequest("error while uploading file");
            }

            return Ok("image uploaded");
        }

        [HttpPut("image/primary/{imageSlug}"), Authorize(Roles = "User")]
        public async Task<IActionResult> SetOfferImageAsPrimary(string imageSlug)
        {
            var userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var offer = await _offerService.GetOfferByOfferImageSlug(imageSlug);

            if (offer == null)
            {
                return BadRequest("image didn't exist");
            }

            if (offer.UserId != userId)
            {
                return BadRequest("permission denied");
            }

            await _offerService.SetOfferImageAsPrimaryBySlug(imageSlug);

            return Ok("image is set as primary");
        }

        [HttpDelete("image/{imageSlug}"), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteOfferImage(string imageSlug)
        {
            var userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var offer = await _offerService.GetOfferByOfferImageSlug(imageSlug);

            if (offer.UserId == Guid.Empty)
            {
                return NotFound("image didn't exist");
            }

            if (offer.UserId != userId)
            {
                return NotFound("permission denied");
            }

            if (!await _fileService.DeleteOfferImage(imageSlug))
            {
                return BadRequest("error while deleting file");
            }

            return Ok("image deleted");
        }
    }
}
