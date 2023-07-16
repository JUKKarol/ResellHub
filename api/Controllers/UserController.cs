using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Enums;
using ResellHub.Services.FileServices;
using ResellHub.Services.OfferServices;
using ResellHub.Services.UserServices;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace ResellHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOfferService _offerService;
        private readonly IFileService _fileService;

        public UserController(IUserService userService, IOfferService offerService, IFileService fileService)
        {
            _userService = userService;
            _offerService = offerService;
            _fileService = fileService;
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

            return Ok(await _userService.GetUserBySlugIncludeAvatar(userSlug));
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
            if (!await _userService.CheckIsUserExistByEmail(userDto.Email))
            {
                return BadRequest("email is already in use");
            }

            return Ok(_userService.CreateUser(userDto));
        }

        [HttpPut("{userId}/{phonenumber}"), Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateUserPhoneNumber(string newPhoneNumber)
        {
            var userId = await _userService.GetUserIdByEmail(HttpContext.User.FindFirstValue(ClaimTypes.Email));

            if (!await _userService.CheckIsUserExistById(userId))
            {
                return BadRequest("user doesn't exist");
            }

            return Ok(await _userService.UpdatePhoneNumber(userId, newPhoneNumber));
        }

        [HttpPut("{userId}/{city}"), Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateUserCity(string newCity)
        {
            var userId = await _userService.GetUserIdByEmail(HttpContext.User.FindFirstValue(ClaimTypes.Email));

            if (!await _userService.CheckIsUserExistById(userId))
            {
                return BadRequest("user doesn't exist");
            }

            return Ok(await _userService.UpdateCity(userId, newCity));
        }

        [HttpDelete("{userId}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            if (!await _userService.CheckIsUserExistById(userId))
            {
                return BadRequest("user doesn't exist");
            }

            return Ok(await _userService.DeleteUser(userId));
        }

        [HttpDelete, Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteAccount()
        {
            return Ok(await _userService.DeleteUser(await _userService.GetUserIdByEmail(HttpContext.User.FindFirstValue(ClaimTypes.Email))));
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
                return NotFound("user have avatar alredy");
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
