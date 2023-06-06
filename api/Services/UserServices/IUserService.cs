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
        //Message
        Task<string> SendMessage(Guid fromUserId, Guid ToUserId, string content);
        Task<List<Message>> ShowUsersMessages(Guid firstUser, Guid secondUser);
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
