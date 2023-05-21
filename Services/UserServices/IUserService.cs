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
        Task<string> CreateUser(UserRegistrationDto user);
        Task<string> UpdatePhoneNumber(Guid userId, string phoneNumber);
        Task<string> UpdateCity(Guid userId, string city);
        Task<string> DeleteUser(Guid userId);
        //Message
        Task<string> SendMessage(Guid fromUserId, Guid ToUserId, string content);
        Task<dynamic> ShowUsersMessages(Guid firstUser, Guid secondUser);
        //Role
        Task<List<Role>> GetUserRoles(Guid userId);
        Task<string> AddRole(Guid userId, UserRoles userRole);
        Task<string> UpdateRole(Guid roleId, UserRoles userNewRole);
        Task<string> DeleteRole(Guid roleId);
        //FollowOffer
        Task<dynamic> GetUserFollowingOffers(Guid userId);
        Task<string> AddOfferToFollowing(Guid userId, Guid offerId);
        Task<string> RemoveOfferFromFollowing(Guid followingOfferId);
    }
}
