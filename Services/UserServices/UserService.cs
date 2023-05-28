﻿using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ResellHub.Data.Repositories.UserRepository;
using ResellHub.DTOs.OfferDTOs;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Enums;
using ResellHub.Services.EmailService;
using ResellHub.Utilities.UserUtilities;

namespace ResellHub.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserUtilities _userUtilities;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IValidator<UserRegistrationDto> _userValidator;

        public UserService(IUserRepository userRepository, IUserUtilities userUtilities, IEmailService emailService, IConfiguration configuration, IMapper mapper, IValidator<UserRegistrationDto> userValidator)
        {
            _userRepository = userRepository;
            _userUtilities = userUtilities;
            _emailService = emailService;
            _configuration = configuration;
            _mapper = mapper;
            _userValidator = userValidator;
        }

        //User
        public async Task<List<UserPublicDto>> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            var usersDto = _mapper.Map<List<UserPublicDto>>(users);

            return usersDto;
        }

        public async Task<UserPublicDto> GetUserById(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);
            var userDto = _mapper.Map<UserPublicDto>(user);

            return userDto;
        }

        public async Task<bool> CheckIsUserExistById(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> CheckIsUserExistByEmail(string userEmail)
        {
            var user = await _userRepository.GetUserByEmail(userEmail);

            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<string> CreateUser(UserRegistrationDto userDto)
        {
            var validationResult = await _userValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(error => error.ErrorMessage);
                return string.Join(Environment.NewLine, validationErrors);
            }

            var user = _mapper.Map<User>(userDto);
            user.EncodeName();

            if (await _userRepository.GetUserByEncodedName(user.EncodedName) != null)
            {
                int randomNumber = new Random().Next(1, 10000);
                user.EncodedName = $"{user.EncodedName}-{randomNumber}";
            }

            byte[] passwordHash;
            byte[] passwordSalt;
            _userUtilities.CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.VeryficationToken = _userUtilities.CreateRandomToken();

            await _userRepository.AddUser(user);

            var userBasicRole = new Role { UserId = user.Id };
            await _userRepository.CreateRole(userBasicRole);

            _emailService.SendVeryficationToken(userDto.Email, user.VeryficationToken);

            return "User ceated successful, email with conformation token was sent";
        }

        public async Task<string> LoginUser(UserLoginDto userDto)
        {
            var user = await _userRepository.GetUserByEmail(userDto.Email);

            if (user == null)
            {
                return "User not found";
            }

            if (!_userUtilities.VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return "Password incorect";
            }

            if (user.VerifiedAt == null)
            {
                return "Not verified";
            }

            string token = await _userUtilities.CreateToken(userDto);

            return token;
        }

        public async Task<string> VerifyUser(string token)
        {
            var user = await _userRepository.GetUserByVeryficationToken(token);
            if (user == null)
            {
                return "Invalid token";
            }

            user.VerifiedAt = DateTime.Now;
            await _userRepository.UpdateUser(user.Id, user);

            return "User verified";
        }

        public async Task<string> ForgotPassword(string userEmail)
        {
            var user = await _userRepository.GetUserByEmail(userEmail);
            if (user == null)
            {
                return "User not found";
            }

            user.PasswordResetToken = _userUtilities.CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);

            _emailService.SendPasswordResetToken(userEmail, user.PasswordResetToken);

            await _userRepository.UpdateUser(user.Id, user);

            return "Reset code was sent to your email";
        }

        public async Task<string> ResetPassword(UserResetPasswordDto userDto)
        {
            var user = await _userRepository.GetUserByResetToken(userDto.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return "Invalid Token.";
            }

            _userUtilities.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _userRepository.UpdateUser(user.Id, user);

            return "Password successfully reset.";
        }

        public async Task<string> UpdatePhoneNumber(Guid userId, string phoneNumber)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return "User didn't exist";
            }

            user.PhoneNumber = phoneNumber;

            await _userRepository.UpdateUser(userId, user);
            return "User updated successful";
        }

        public async Task<string> UpdateCity(Guid userId, string city)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return "User didn't exist";
            }

            user.City = city;

            await _userRepository.UpdateUser(userId, user);
            return "User updated successful";
        }

        public async Task<string> DeleteUser(Guid userId)
        {
            if (await _userRepository.GetUserById(userId) == null)
            {
                return "User didn't exist";
            }

            await _userRepository.DeleteUser(userId);
            return "User deleted successful";
        }

        //Message
        public async Task<string> SendMessage(Guid fromUserId, Guid ToUserId, string content)
        {
            var message = new Message { FromUserId = fromUserId, ToUserId = ToUserId, Content = content };

            await _userRepository.AddMessage(message);

            return "Message send successful";
        }

        public async Task<List<Message>> ShowUsersMessages(Guid firstUser, Guid secondUser)
        {
            return await _userRepository.GetMessagesBetweenTwoUsers(firstUser, secondUser);
        }

        //Role
        public async Task<List<Role>> GetUserRoles(Guid userId)
        {
            return await _userRepository.GetUserRoles(userId);
        }

        public async Task<bool> CheckIsRoleExistById(Guid roleId)
        {
            var role = await _userRepository.GetRoleById(roleId);

            if (role == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<string> AddRole(Guid userId, UserRoles userRole)
        {
            var role = new Role { UserId = userId, UserRole = userRole };

            await _userRepository.CreateRole(role);

            return "Role created successfuly";
        }

        public async Task<string> UpdateRole(Guid roleId, UserRoles userNewRole)
        {
            await _userRepository.ChangeRole(roleId, userNewRole);

            return "Role changed successful";
        }

        public async Task<string> DeleteRole(Guid roleId)
        {
            await _userRepository.DeleteRole(roleId);
            return "Role deleted successful";
        }

        //FollowOffer
        public async Task<List<FollowOffer>> GetUserFollowingOffers(Guid userId)
        {
            return await _userRepository.GetUserFollowingOffers(userId);
        }

        public async Task<bool> CheckIsFollowingExistById(Guid followingOfferId)
        {
            var followOffer = await _userRepository.GetUserFollowingOfferById(followingOfferId);

            if (followOffer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<string> AddOfferToFollowing(Guid userId, Guid offerId)
        {
            var followingOffer = new FollowOffer { UserId = userId, OfferId = offerId };
            await _userRepository.AddFollowingOffer(followingOffer);

            return "Offer is following from now";
        }

        public async Task<string> RemoveOfferFromFollowing(Guid followingOfferId)
        {
            await _userRepository.DeleteFollowingOffer(followingOfferId);
            return "Offer is not following anymore";
        }
    }
}
