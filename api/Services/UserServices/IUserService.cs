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
        Task<string> GetUserEmailById(Guid userId);
        Task<bool> CheckIsUserExistById(Guid userId);
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
        Task<List<Chat>> GetUserChats(Guid userId);
        Task<bool> CheckIsChatExistsById(Guid chatId);
        Task<bool> CheckIsChatExistsByUsersId(Guid firstUserId, Guid secondUserId);
        Task<Chat> GetChatById(Guid chatId);
        Task<String> CreateChat(Guid fromUserId, Guid ToUserId);
        //Message
        Task<List<Message>> GetMessagesByChatId(Guid ChatId);
        Task<string> SendMessage(Guid fromUserId, Guid ToUserId, string content);
        //Role
        Task<List<Role>> GetUserRoles(Guid userId);
        Task<bool> CheckIsRoleExistById(Guid roleId);
        Task<string> AddRole(Guid userId, UserRoles userRole);
        Task<string> UpdateRole(Guid roleId, UserRoles userNewRole);
        Task<string> DeleteRole(Guid roleId);
        //FollowOffer
        Task<List<FollowOffer>> GetUserFollowingOffers(Guid userId);
        Task<FollowOffer> GetFollowingOfferByUserAndOfferId(Guid userId, Guid offerId);
        Task<bool> CheckIsFollowingExistById(Guid followingOfferId);
        Task<string> AddOfferToFollowing(Guid userId, Guid offerId);
        Task<string> RemoveOfferFromFollowing(Guid followingOfferId);
    }
}
