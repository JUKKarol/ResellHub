﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Enums;
using ResellHub.Services.OfferServices;
using ResellHub.Services.SupabaseServices;
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
        private readonly ISupabaseService _supabaseService;

        public UserController(IUserService userService, IOfferService offerService, ISupabaseService supabaseService)
        {
            _userService = userService;
            _offerService = offerService;
            _supabaseService = supabaseService;
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
        [HttpPost("avatar"), Authorize(Roles = "User")]
        public async Task<IActionResult> UploadAvatar(IFormFile image)
        {
            Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!await _userService.CheckIsAvatarImageExistByUserId(userId))
            { 
                return BadRequest("user have avatar alredy");
            }

            if (!await _supabaseService.UploadAvatar(image, userId))
            { 
                return BadRequest("failed while uploading avatar");
            }

            return Ok("avatar uploaded");
        }

        [HttpDelete("avatar"), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteAvatar(string avatarSlug)
        {
            //get image by slug
            //check permission

            //delete photo service

            return Ok();
        }
    }
}
