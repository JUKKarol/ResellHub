using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Services.FileServices;
using ResellHub.Services.OfferServices;
using ResellHub.Services.UserServices;
using Sieve.Models;
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
        private readonly IFileService _fileService;

        public UserController(
            IUserService userService,
            IOfferService offerService,
            IFileService fileService,
            IValidator<UserRegistrationDto> userRegistrationValidator,
            IValidator<UserUpdateDto> userUpdateValidator)
        {
            _userService = userService;
            _offerService = offerService;
            _fileService = fileService;
            _userRegistrationValidator = userRegistrationValidator;
            _userUpdateValidator = userUpdateValidator;
        }

        [HttpGet, Authorize(Roles = "User")]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1)
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

            return Ok(await _userService.GetUserBySlugIncludeAvatar(userSlug));
        }

        [HttpPost("{userSlug}/offers"), Authorize(Roles = "User"), AllowAnonymous]
        public async Task<IActionResult> GetUserOffersBySlug(string userSlug, [FromBody] SieveModel query)
        {
            if (!await _userService.CheckIsUserExistBySlug(userSlug))
            {
                return BadRequest("user doesn't exist");
            }

            Guid loggedUserId = User.Identity.IsAuthenticated ? Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty;

            return Ok(await _offerService.GetUserOffers(userSlug, query, loggedUserId));
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

        //images
        [HttpGet("avatar"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetMyAvatar()
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!await _userService.CheckIsAvatarImageExistByUserId(userId))
            {
                return NotFound("user didn't uploaded avatar yet");
            }

            byte[] myAvatar = await _fileService.GetAvatar(userId);

            if (myAvatar.Length < 1)
            {
                return BadRequest("error while uploading file");
            }

            return Ok(myAvatar);
        }

        [HttpPost("avatar"), Authorize(Roles = "User")]
        public async Task<IActionResult> UploadAvatar(IFormFile image)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (await _userService.CheckIsAvatarImageExistByUserId(userId))
            {
                return NotFound("user have avatar already");
            }

            if (image == null)
            {
                return BadRequest("image can't be empty");
            }

            if (!_fileService.CheckIsAvatarSizeCorrect(image))
            {
                return BadRequest("image is to large");
            }

            if (!await _fileService.AddAvatar(image, userId))
            {
                return BadRequest("error while uploading file");
            }

            return Ok("avatar uploaded");
        }

        [HttpDelete("avatar"), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteAvatar()
        {
            var userAvatar = await _userService.GetAvatarByUserId(Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            if (userAvatar == null)
            {
                return NotFound("user didn't uploaded avatar yet");
            }

            if (!await _fileService.DeleteAvatar(userAvatar.UserId))
            {
                return BadRequest("error while deleting file");
            }

            return Ok("avatar deleted");
        }
    }
}