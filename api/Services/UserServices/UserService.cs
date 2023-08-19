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
using ResellHub.Services.FileServices;
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
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IValidator<UserRegistrationDto> _userValidator;

        public UserService(IUserRepository userRepository, IUserUtilities userUtilities, IEmailService emailService, IFileService fileService, IConfiguration configuration, IMapper mapper, IValidator<UserRegistrationDto> userValidator)
        {
            _userRepository = userRepository;
            _userUtilities = userUtilities;
            _emailService = emailService;
            _fileService = fileService;
            _configuration = configuration;
            _mapper = mapper;
            _userValidator = userValidator;
        }

        //User
        public async Task<UserRespondListDto> GetUsers(int page)
        {
            int pageSize = 15;
            var users = await _userRepository.GetUsers(page, pageSize);
            var usersDto = _mapper.Map<List<UserPublicDto>>(users);

            UserRespondListDto userRespondListDto = new UserRespondListDto();
            userRespondListDto.Users = usersDto;
            userRespondListDto.UsersCount = await _userRepository.GetUsersCount();
            userRespondListDto.CurrentPage = page;
            userRespondListDto.PagesCount = (int)Math.Ceiling((double)userRespondListDto.UsersCount / pageSize);

            return userRespondListDto;
        }

        public async Task<UserPublicDto> GetUserById(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);
            var userDto = _mapper.Map<UserPublicDto>(user);

            return userDto;
        }

        public async Task<UserDetalisDto> GetUserBySlugIncludeAvatar(string userSlug)
        {
            var user = await _userRepository.GetUserBySlugIncludeAvatar(userSlug);
            var userDto = _mapper.Map<UserDetalisDto>(user);

            userDto.Avatar = await _fileService.GetAvatar(user.AvatarImage.UserId);

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

        public async Task<bool> CheckIsUserExistBySlug(string userSlug)
        {
            var user = await _userRepository.GetUserBySlug(userSlug);

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

        public async Task<bool> CreateUser(UserRegistrationDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.EncodeName();

            if (await _userRepository.GetUserBySlug(user.Slug) != null)
            {
                int randomNumber = new Random().Next(1, 10000);
                user.Slug = $"{user.Slug}-{randomNumber}";
            }

            if (await _userRepository.GetUserBySlug(user.Slug) != null)
            {
                return false;
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

            return true;
        }

        public async Task<string> LoginUser(UserLoginDto userDto)
        {
            string token = await _userUtilities.CreateToken(userDto);

            return token;
        }

        public async Task VerifyUser(string token)
        {
            var user = await _userRepository.GetUserByVeryficationToken(token);

            user.VerifiedAt = DateTime.UtcNow;
            await _userRepository.UpdateUser(user.Id, user);
        }

        public async Task ForgotPassword(string userEmail)
        {
            var user = await _userRepository.GetUserByEmail(userEmail);

            user.PasswordResetToken = _userUtilities.CreateRandomToken();
            user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

            _emailService.SendPasswordResetToken(userEmail, user.PasswordResetToken);

            await _userRepository.UpdateUser(user.Id, user);
        }

        public async Task ResetPassword(UserResetPasswordDto userDto)
        {
            var user = await _userRepository.GetUserByResetToken(userDto.Token);

            _userUtilities.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _userRepository.UpdateUser(user.Id, user);
        }

        public async Task UpdateUser(Guid userId, UserUpdateDto userDto)
        {
            var user = await _userRepository.GetUserById(userId);

            if (!string.IsNullOrEmpty(userDto.PhoneNumber))
            {
                user.PhoneNumber = userDto.PhoneNumber;
            }

            if (!string.IsNullOrEmpty(userDto.City))
            {
                user.City = userDto.City;
            }

            if (!string.IsNullOrEmpty(userDto.Email))
            {
                user.Email = userDto.Email;
            }

            await _userRepository.UpdateUser(userId, user);
        }

        public async Task DeleteUser(Guid userId)
        {
            await _userRepository.DeleteUser(userId);
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

        public async Task AddRole(Guid userId, UserRoles userRole)
        {
            var role = new Role { UserId = userId, UserRole = userRole };

            await _userRepository.CreateRole(role);
        }

        public async Task UpdateRole(Guid roleId, UserRoles userNewRole)
        {
            await _userRepository.ChangeRole(roleId, userNewRole);
        }

        public async Task DeleteRole(Guid roleId)
        {
            await _userRepository.DeleteRole(roleId);
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

        public async Task AddOfferToFollowing(Guid userId, Guid offerId)
        {
            var followingOffer = new FollowOffer { UserId = userId, OfferId = offerId };
            await _userRepository.AddFollowingOffer(followingOffer);
        }

        public async Task RemoveOfferFromFollowing(Guid followingOfferId)
        {
            await _userRepository.DeleteFollowingOffer(followingOfferId);
        }

        //AvatarImage
        public async Task<bool> CheckIsAvatarImageExistByUserId(Guid userId)
        {
            var avatarImage = await _userRepository.GetAvatarImageByUserId(userId);

            if (avatarImage != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<AvatarImage> GetAvatarByUserId(Guid userId)
        {
            return await _userRepository.GetAvatarImageByUserId(userId);
        }
    }
}
