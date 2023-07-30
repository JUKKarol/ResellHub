using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Enums;
using ResellHub.Services.OfferServices;
using ResellHub.Services.UserServices;
using System.Security.Claims;

namespace ResellHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOfferService _offerService;
        private readonly IValidator<UserRegistrationDto> _userRegistrationValidator;
        private readonly IValidator<UserUpdateDto> _userUpdateValidator;

        public UserController(IUserService userService, IOfferService offerService, IValidator<UserRegistrationDto> userRegistrationValidator, IValidator<UserUpdateDto> userUpdateValidator)
        {
            _userService = userService;
            _offerService = offerService;
            _userRegistrationValidator = userRegistrationValidator;
            _userUpdateValidator = userUpdateValidator;
        }

        [HttpGet, Authorize(Roles = "User")]
        public async Task<IActionResult> GetUsers(int page = 1)
        {
            return Ok(await _userService.GetUsers(page));
        }

        [HttpGet("{userSlug}"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserBySlug(string userSlug)
        {
            if (!await _userService.CheckIsUserExistBySlug(userSlug))
            {
                return BadRequest("user doesn't exist");
            }

            return Ok(await _userService.GetUserBySlug(userSlug));
        }

        [HttpGet("{userSlug}/offers"), Authorize(Roles = "User"), AllowAnonymous]
        public async Task<IActionResult> GetUserOffersBySlug(string userSlug, int page = 1)
        {
            if (!await _userService.CheckIsUserExistBySlug(userSlug))
            {
                return BadRequest("user doesn't exist");
            }

            Guid loggedUserId = User.Identity.IsAuthenticated ? Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty;

            return Ok(await _offerService.GetUserOffers(userSlug, page, loggedUserId));
        }

        [HttpPost, Authorize(Roles = "Moderator")]
        public async Task<IActionResult> CreateUser(UserRegistrationDto userDto)
        {
            var validationResult = await _userRegistrationValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(error => error.ErrorMessage);
                return BadRequest(string.Join(Environment.NewLine, validationErrors));
            }

            if (!await _userService.CheckIsUserExistByEmail(userDto.Email))
            {
                return BadRequest("email is already in use");
            }

            return Ok(_userService.CreateUser(userDto));
        }

        [HttpPut(), Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.City) && string.IsNullOrEmpty(userDto.Email) && string.IsNullOrEmpty(userDto.PhoneNumber))
            {
                return BadRequest("no data to update");
            }

            var validationResult = await _userUpdateValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(error => error.ErrorMessage);
                return BadRequest(string.Join(Environment.NewLine, validationErrors));
            }

            await _userService.UpdateUser(Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)), userDto);

            return Ok("profile updated successfully");
        }

        [HttpDelete("{userId}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            if (!await _userService.CheckIsUserExistById(userId))
            {
                return BadRequest("user doesn't exist");
            }

            await _userService.DeleteUser(userId);

            return Ok("account deleted");
        }

        [HttpDelete, Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteAccount()
        {
            await _userService.DeleteUser(Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            return Ok("account deleted");
        }
    }
}
