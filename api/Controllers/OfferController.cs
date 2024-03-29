﻿using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResellHub.DTOs.OfferDTOs;
using ResellHub.Services.FileServices;
using ResellHub.Services.OfferServices;
using ResellHub.Services.UserServices;
using Sieve.Models;
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
        private readonly IValidator<OfferUpdateDto> _offerUpdateValidator;
        private readonly IFileService _fileService;

        public OfferController(
            IOfferService offerService,
            IUserService userService,
            IValidator<OfferCreateDto> offerCreateValidator,
            IValidator<OfferUpdateDto> offerUpdateValidator,
            IFileService fileService)
        {
            _offerService = offerService;
            _userService = userService;
            _offerCreateValidator = offerCreateValidator;
            _offerUpdateValidator = offerUpdateValidator;
            _fileService = fileService;
        }

        [HttpGet, Authorize(Roles = "User"), AllowAnonymous]
        public async Task<IActionResult> GetOffers([FromQuery] SieveModel query)
        {
            Guid loggedUserId = User.Identity.IsAuthenticated ? Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty;
            var offers = await _offerService.GetOffers(query, loggedUserId);

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

        [HttpPost("create"), Authorize(Roles = "User")]
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

        [HttpPut("{offerSlug}"), Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateOffer(string offerSlug, OfferUpdateDto offerDto)
        {
            var validationResult = await _offerUpdateValidator.ValidateAsync(offerDto);
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(error => error.ErrorMessage);
                return BadRequest(string.Join(Environment.NewLine, validationErrors));
            }

            var offer = await _offerService.GetOfferBySlug(offerSlug, Guid.Empty);

            if (offer == null)
            {
                return BadRequest("offer doesn't exist");
            }

            if (offer.UserId != Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return BadRequest("you aren't offer owner");
            }

            await _offerService.UpdateOffer(offerSlug, offerDto);

            return Ok("offer updated");
        }

        [HttpDelete("{offerSlug}"), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteOffer(string offerSlug)
        {
            var offer = await _offerService.GetOfferBySlug(offerSlug, Guid.Empty);

            if (offer == null)
            {
                return BadRequest("offer doesn't exist");
            }

            if (offer.UserId != Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return BadRequest("you aren't offer owner");
            }

            return Ok("offer deleted");
        }

        //images
        [HttpGet("{offerSlug}/image"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetOfferImages(string offerSlug)
        {
            var offer = await _offerService.GetOfferBySlug(offerSlug, Guid.Empty);

            if (offer == null)
            {
                return NotFound("offer didn't exist");
            }

            if (!offer.OfferImages.Any())
            {
                return NotFound("offer didn't have uploaded images yet");
            }

            var offerImages = await _fileService.GetOfferImagesByOfferSlug(offerSlug);

            foreach (var offerImage in offerImages)
            {
                if (offerImage.ImageBytes.Length < 1)
                {
                    return BadRequest("error while uploading file");
                }
            }

            return Ok(offerImages);
        }

        [HttpPost("{offerSlug}/image"), Authorize(Roles = "User")]
        public async Task<IActionResult> UploadOfferImages(List<IFormFile> images, string offerSlug)
        {
            var userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var offer = await _offerService.GetOfferBySlug(offerSlug, Guid.Empty);

            if (offer.UserId != userId)
            {
                return BadRequest("permission denied");
            }

            if (images == null)
            {
                return BadRequest("images can't be empty");
            }

            if ((images.Count + offer.OfferImages.Count) > _fileService.MaxImagesForOffer)
            {
                return BadRequest("too much images in request");
            }

            if (offer.OfferImages.Count > _fileService.MaxImagesForOffer)
            {
                return BadRequest("offer images are full");
            }

            foreach (var image in images)
            {
                if (!_fileService.CheckIsOfferImageSizeCorrect(image))
                {
                    return BadRequest("image is to large");
                }
            }

            if (!await _fileService.AddOfferImages(images, await _offerService.GetOfferIdByOfferSlug(offerSlug)))
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