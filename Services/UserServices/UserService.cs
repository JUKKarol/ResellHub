using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ResellHub.Data.Repositories.UserRepository;
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

        public UserService(IUserRepository userRepository, IUserUtilities userUtilities, IEmailService emailService, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _userUtilities = userUtilities;
            _emailService = emailService;
            _configuration = configuration;
            _mapper = mapper;
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

        public async Task<string> CreateUser(UserRegistrationDto userDto)
        {
            if(await _userRepository.GetUserByEmail(userDto.Email) != null)
            {
                return "User already exist";
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

            return "User ceated successful";
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

            return "User verified";
        }

        public async Task<string> ForgotPassword(string token)
        {
            return "";
        }

        public async Task<string> ResetPassword(string token)
        {
            return "";
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
            if (await _userRepository.GetUserById(ToUserId) == null)
            {
                return "Sender didn't exist";
            }

            if (await _userRepository.GetUserById(ToUserId) == null)
            {
                return "Reciver didn't exist";
            }

            var message = new Message { FromUserId = fromUserId, ToUserId = ToUserId, Content = content };

            await _userRepository.AddMessage(message);

            return "Message send successful";
        }

        public async Task<dynamic> ShowUsersMessages(Guid firstUser, Guid secondUser)
        {
            if (await _userRepository.GetUserById(firstUser) == null)
            {
                return "First user didn't exist";
            }

            if (await _userRepository.GetUserById(secondUser) == null)
            {
                return "Second user didn't exist";
            }

            var messages = await _userRepository.GetMessagesBetweenTwoUsers(firstUser, secondUser);
            return messages;
        }

        //Role
        public async Task<List<Role>> GetUserRoles(Guid userId)
        {
           var roles = await _userRepository.GetUserRoles(userId);

            return roles;
        }

        public async Task<string> AddRole(Guid userId, UserRoles userRole)
        {
            if (await _userRepository.GetUserById(userId) == null)
            {
                return "User didn't exist";
            }

            var role = new Role { UserId = userId, UserRole = userRole };

            await _userRepository.CreateRole(role);

            return "Role created successfuly";
        }

        public async Task<string> UpdateRole(Guid roleId, UserRoles userNewRole)
        {
            var existRole = await _userRepository.GetRoleById(roleId);

            if (existRole == null)
            {
                return "Role didn't exist";
            }

            await _userRepository.ChangeRole(roleId, userNewRole);

            return "Role changed successful";
        }

        public async Task<string> DeleteRole(Guid roleId)
        {
            if (await _userRepository.GetRoleById(roleId) == null)
            {
                return "Role didn't exist";
            }

            await _userRepository.DeleteRole(roleId);
            return "Role deleted successful";
        }

        //FollowOffer
        public async Task<dynamic> GetUserFollowingOffers(Guid userId)
        {
            if (_userRepository.GetUserById(userId) == null)
            {
                return "User didn't exist";
            }

            var userFollowingOffers = await _userRepository.GetUserFollowingOffers(userId);

            return userFollowingOffers;
        }

        public async Task<string> AddOfferToFollowing(Guid userId, Guid offerId)
        {
            if (_userRepository.GetUserById(userId) == null)
            {
                return "User didn't exist";
            }

            //Check is offer eixst

            var followingOffer = new FollowOffer { UserId = userId, OfferId = offerId };
            await _userRepository.AddFollowingOffer(followingOffer);

            return "Offer is following from now";
        }

        public async Task<string> RemoveOfferFromFollowing(Guid followingOfferId)
        {
            var followingOffer = _userRepository.GetUserFollowingOfferById(followingOfferId);

            if (followingOffer == null)
            {
                return "Following Offer didn't exist";
            }

            await _userRepository.DeleteFollowingOffer(followingOfferId);
            return "Offer is not following anymore";
        }
    }
}
