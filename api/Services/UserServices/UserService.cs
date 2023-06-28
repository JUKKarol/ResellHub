using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ResellHub.Data.Repositories.UserRepository;
using ResellHub.DTOs.ChatDTOs;
using ResellHub.DTOs.FollowOfferDTOs;
using ResellHub.DTOs.MessageDTOs;
using ResellHub.DTOs.OfferDTOs;
using ResellHub.DTOs.RoleDTOs;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Enums;
using ResellHub.Services.EmailService;
using ResellHub.Utilities.UserUtilities;
using System;
using System.Collections.Generic;

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
        public async Task<List<UserPublicDto>> GetUsers(int page)
        {
            var users = await _userRepository.GetUsers(page, 15);
            var usersDto = _mapper.Map<List<UserPublicDto>>(users);

            return usersDto;
        }

        public async Task<UserPublicDto> GetUserById(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);
            var userDto = _mapper.Map<UserPublicDto>(user);

            return userDto;
        }

        public async Task<string> GetUserEmailById(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);
            var userEmail = user.Email;

            return userEmail;
        }

        public async Task<Guid> GetUserIdByEmail(string userEmail)
        {
            var user = await _userRepository.GetUserByEmail(userEmail);
            var userId = user.Id;

            return userId;
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

            if (await _userRepository.GetUserBySlug(user.Slug) != null)
            {
                int randomNumber = new Random().Next(1, 10000);
                user.Slug = $"{user.Slug}-{randomNumber}";
            }

            if (await _userRepository.GetUserBySlug(user.Slug) != null)
            {
                return "Name is already in use";
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

            user.VerifiedAt = DateTime.UtcNow;
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
            user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

            _emailService.SendPasswordResetToken(userEmail, user.PasswordResetToken);

            await _userRepository.UpdateUser(user.Id, user);

            return "Reset code was sent to your email";
        }

        public async Task<string> ResetPassword(UserResetPasswordDto userDto)
        {
            var user = await _userRepository.GetUserByResetToken(userDto.Token);
            if (user == null || user.ResetTokenExpires < DateTime.UtcNow)
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

        //Chat
        public async Task<List<ChatDto>> GetUserChats(Guid userId, int page)
        {
            var chats = await _userRepository.GetUserChats(userId, page, 15);
            var chatsDto = _mapper.Map<List<ChatDto>>(chats);

            return chatsDto;
        }

        public async Task<bool> CheckIsChatExistsById(Guid chatId)
        {
            if (await _userRepository.GetChatById(chatId) == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckIsChatExistsByUsersId(Guid firstUserId, Guid secondUserId)
        {
            if (await _userRepository.GetChatByUsersId(firstUserId, secondUserId) == null)
            {
                return false;
            }
            return true;
        }

        public async Task<Chat> GetChatById(Guid chatId)
        {
            return await _userRepository.GetChatById(chatId);
        }

        public async Task<Chat> CreateChat(Guid senderId, Guid ReciverId)
        {
            return await _userRepository.CreateChat(senderId, ReciverId);
        }

        //Message
        public async Task<List<MessageDisplayDto>> GetMessagesByChatId(Guid ChatId, int page)
        {
            var messages = await _userRepository.GetChatMessagesById(ChatId, page, 15);
            var messagesDto = _mapper.Map<List<MessageDisplayDto>>(messages);

            return messagesDto;
        }

        public async Task<string> SendMessage(Guid chatId, Guid senderId, string content)
        {
            var chat = await _userRepository.GetChatById(chatId);
            Guid reciverId;

            if (chat.SenderId == senderId)
            {
                reciverId = chat.ReciverId;
            }
            else
            {
                reciverId = chat.SenderId;
            }

            var message = new Message { ChatId = chatId, SenderId = senderId, ReciverId = reciverId, Content = content };

            await _userRepository.AddMessage(message);
            await _userRepository.RefreshChatLastMessageAt(chatId);

            return "Message send successful";
        }

        //Role
        public async Task<List<RoleDto>> GetUserRoles(Guid userId)
        {
            var userRoles = await _userRepository.GetUserRoles(userId);
            var userRolesDto = _mapper.Map<List<RoleDto>>(userRoles);

            return userRolesDto;
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
        public async Task<List<FollowOfferDto>> GetUserFollowingOffers(Guid userId, int page)
        {
            var followingOffers = await _userRepository.GetUserFollowingOffers(userId, page, 40);
            var followingOffersDto = _mapper.Map<List<FollowOfferDto>>(followingOffers);

            return followingOffersDto;
        }

        public async Task<FollowOfferDto> GetFollowingOfferByUserAndOfferId(Guid userId, Guid offerId)
        {
            var userFollowingOffers = await _userRepository.GetFollowingOfferByUserAndOfferId(userId, offerId);
            var userFollowingOffersDto = _mapper.Map<FollowOfferDto>(userFollowingOffers);

            return userFollowingOffersDto;
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
