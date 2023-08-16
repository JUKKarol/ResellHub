using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResellHub.Data.Repositories.UserRepository;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Services.UserServices;
using ResellHub.Utilities.UserUtilities;

namespace ResellHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IUserUtilities _userUtilities;
        private readonly IValidator<UserRegistrationDto> _userRegistrationValidator;
        private readonly IValidator<UserResetPasswordDto> _userResetPasswordValidator;

        public AuthController(
            IUserService userService,
            IUserRepository userRepository,
            IUserUtilities userUtilities,
            IValidator<UserRegistrationDto> userRegistrationValidator,
            IValidator<UserResetPasswordDto> userResetPasswordValidator)
        {
            _userService = userService;
            _userRepository = userRepository;
            _userUtilities = userUtilities;
            _userRegistrationValidator = userRegistrationValidator;
            _userResetPasswordValidator = userResetPasswordValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto userDto)
        {
            var validationResult = await _userRegistrationValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(error => error.ErrorMessage);
                return BadRequest(string.Join(Environment.NewLine, validationErrors));
            }

            if (!await _userService.CreateUser(userDto))
            {
                return BadRequest("error while creating account");
            }

            return Ok(await _userService.CreateUser(userDto));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginOwner(UserLoginDto userDto)
        {
            var user = await _userRepository.GetUserByEmail(userDto.Email);

            if (user == null)
            {
                return NotFound("user not found");
            }

            if (!_userUtilities.VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Password incorrect");
            }

            if (user.VerifiedAt == null)
            {
                return BadRequest("not verified");
            }

            return Ok(await _userService.LoginUser(userDto));
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            var user = await _userRepository.GetUserByVeryficationToken(token);

            if (user == null)
            {
                return BadRequest("token incorrect");
            }

            if (user.VerifiedAt == null)
            {
                return NotFound("account is already verified");   
            }

            await _userService.VerifyUser(token);
            return Ok("account verified");

        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string userEmail)
        {
            var user = await _userRepository.GetUserByEmail(userEmail);
            if (user == null)
            {
                return NotFound("user not found");
            }

            await _userService.ForgotPassword(userEmail);
            return Ok("email sent successful");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(UserResetPasswordDto userDto)
        {
            var validationResult = await _userResetPasswordValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(error => error.ErrorMessage);
                return BadRequest(string.Join(Environment.NewLine, validationErrors));
            }

            var user = await _userRepository.GetUserByResetToken(userDto.Token);
            if (user == null || user.ResetTokenExpires < DateTime.UtcNow)
            {
                return BadRequest("invalid Token.");
            }

            await _userService.ResetPassword(userDto);
            return Ok("password reset successfully");
        }
    }
}
