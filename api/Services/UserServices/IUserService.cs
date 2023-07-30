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
        Task<List<UserPublicDto>> GetUsers(int page);
        Task<UserPublicDto> GetUserById(Guid userId);
        Task<UserPublicDto> GetUserBySlug(string userSlug);
        Task<string> GetUserEmailById(Guid userId);
        Task<bool> CheckIsUserExistById(Guid userId);
        Task<bool> CheckIsUserExistBySlug(string userSlug);
        Task<bool> CheckIsUserExistByEmail(string userEmail);
        Task<Guid> GetUserIdByEmail(string userEmail);
        Task<bool> CreateUser(UserRegistrationDto user);
        Task<string> LoginUser(UserLoginDto userDto);
        Task VerifyUser(string token);
        Task ForgotPassword(string userEmail);
        Task ResetPassword(UserResetPasswordDto userDto);
        Task UpdateUser(Guid userId, UserUpdateDto userDto);
        Task DeleteUser(Guid userId);
        //Chat
        Task<List<ChatDto>> GetUserChats(Guid userId, int page);
        Task<bool> CheckIsChatExistsById(Guid chatId);
        Task<bool> CheckIsChatExistsByUsersId(Guid firstUserId, Guid secondUserId);
        Task<Chat> GetChatById(Guid chatId);
        Task<Chat> CreateChat(Guid senderId, Guid ReciverId);
        //Message
        Task<List<MessageDisplayDto>> GetMessagesByChatId(Guid ChatId, int page);
        Task<string> SendMessage(Guid senderId, Guid ReciverId, string content);
        //Role
        Task<List<RoleDto>> GetUserRoles(Guid userId);
        Task<bool> CheckIsRoleExistById(Guid roleId);
        Task<string> AddRole(Guid userId, UserRoles userRole);
        Task<string> UpdateRole(Guid roleId, UserRoles userNewRole);
        Task<string> DeleteRole(Guid roleId);
        //FollowOffer
        Task<List<FollowOfferDto>> GetUserFollowingOffers(Guid userId, int page);
        Task<FollowOfferDto> GetFollowingOfferByUserAndOfferId(Guid userId, Guid offerId);
        Task<bool> CheckIsFollowingExistById(Guid followingOfferId);
        Task AddOfferToFollowing(Guid userId, Guid offerId);
        Task RemoveOfferFromFollowing(Guid followingOfferId);
    }
}
