using ResellHub.DTOs.ChatDTOs;
using ResellHub.DTOs.FollowOfferDTOs;
using ResellHub.DTOs.MessageDTOs;
using ResellHub.DTOs.RoleDTOs;
using ResellHub.DTOs.SharedDTOs;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Enums;

namespace ResellHub.Services.UserServices
{
    public interface IUserService
    {
        //User
        Task<PagedRespondListDto<UserPublicDto>> GetUsers(int page);
        Task<UserPublicDto> GetUserById(Guid userId);
        Task<UserDetalisDto> GetUserBySlugIncludeAvatar(string userSlug);
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
        Task<PagedRespondListDto<ChatDisplayDto>> GetUserChats(Guid userId, int page);
        Task<bool> CheckIsChatExistsById(Guid chatId);
        Task<bool> CheckIsChatExistsByUsersId(Guid firstUserId, Guid secondUserId);
        Task<Chat> GetChatById(Guid chatId);
        Task<Chat> CreateChat(Guid senderId, Guid ReciverId);
        //Message
        Task<PagedRespondListDto<MessageDisplayDto>> GetMessagesByChatId(Guid ChatId, int page);
        Task<string> SendMessage(Guid senderId, Guid ReciverId, string content);
        //Role
        Task<List<RoleDto>> GetUserRoles(Guid userId);
        Task<bool> CheckIsRoleExistById(Guid roleId);
        Task AddRole(Guid userId, UserRoles userRole);
        Task UpdateRole(Guid roleId, UserRoles userNewRole);
        Task DeleteRole(Guid roleId);
        //FollowOffer
        Task<PagedRespondListDto<FollowOfferDto>> GetUserFollowingOffers(Guid userId, int page);
        Task<FollowOfferDto> GetFollowingOfferByUserAndOfferId(Guid userId, Guid offerId);
        Task<bool> CheckIsFollowingExistById(Guid followingOfferId);
        Task AddOfferToFollowing(Guid userId, Guid offerId);
        Task RemoveOfferFromFollowing(Guid followingOfferId);
        //Avatar
        Task<bool> CheckIsAvatarImageExistByUserId(Guid userId);
        Task<AvatarImage> GetAvatarByUserId(Guid userId);
    }
}
