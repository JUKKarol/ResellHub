using ResellHub.DTOs.ChatDTOs;
using ResellHub.DTOs.FollowOfferDTOs;
using ResellHub.DTOs.MessageDTOs;
using ResellHub.DTOs.RoleDTOs;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Enums;

namespace ResellHub.Services.UserServices
{
    public interface IUserService
    {
        //User
        Task<List<UserPublicDto>> GetUsers();
        Task<UserPublicDto> GetUserById(Guid userId);
        Task<UserPublicDto> GetUserBySlug(string userSlug);
        Task<string> GetUserEmailById(Guid userId);
        Task<bool> CheckIsUserExistById(Guid userId);
        Task<bool> CheckIsUserExistBySlug(string userSlug);
        Task<bool> CheckIsUserExistByEmail(string userEmail);
        Task<Guid> GetUserIdByEmail(string userEmail);
        Task<string> CreateUser(UserRegistrationDto user);
        Task<string> LoginUser(UserLoginDto userDto);
        Task<string> VerifyUser(string token);
        Task<string> ForgotPassword(string userEmail);
        Task<string> ResetPassword(UserResetPasswordDto userDto);
        Task<string> UpdatePhoneNumber(Guid userId, string phoneNumber);
        Task<string> UpdateCity(Guid userId, string city);
        Task<string> DeleteUser(Guid userId);
        //Chat
        Task<List<ChatDto>> GetUserChats(Guid userId, int page);
        Task<bool> CheckIsChatExistsById(Guid chatId);
        Task<bool> CheckIsChatExistsByUsersId(Guid firstUserId, Guid secondUserId);
        Task<Chat> GetChatById(Guid chatId);
        Task<Chat> CreateChat(Guid fromUserId, Guid ToUserId);
        //Message
        Task<List<MessageDisplayDto>> GetMessagesByChatId(Guid ChatId, int page);
        Task<string> SendMessage(Guid fromUserId, Guid ToUserId, string content);
        //Role
        Task<List<RoleDto>> GetUserRoles(Guid userId);
        Task<bool> CheckIsRoleExistById(Guid roleId);
        Task<string> AddRole(Guid userId, UserRoles userRole);
        Task<string> UpdateRole(Guid roleId, UserRoles userNewRole);
        Task<string> DeleteRole(Guid roleId);
        //FollowOffer
        Task<List<FollowOfferDto>> GetUserFollowingOffers(Guid userId);
        Task<FollowOfferDto> GetFollowingOfferByUserAndOfferId(Guid userId, Guid offerId);
        Task<bool> CheckIsFollowingExistById(Guid followingOfferId);
        Task<string> AddOfferToFollowing(Guid userId, Guid offerId);
        Task<string> RemoveOfferFromFollowing(Guid followingOfferId);
    }
}
