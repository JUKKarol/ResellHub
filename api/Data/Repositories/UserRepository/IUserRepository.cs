using ResellHub.Entities;
using ResellHub.Enums;

namespace ResellHub.Data.Repositories.UserRepository
{
    public interface IUserRepository
    {
        //User
        Task<List<User>> GetUsers();
        Task<User> GetUserById(Guid userId);
        Task<User> GetUserByEmail(string userEmail);
        Task<User> GetUserByVeryficationToken(string userToken);
        Task<User> GetUserByResetToken(string userToken);
        Task<User> GetUserByEncodedName(string userEncodedName);
        Task<User> AddUser(User user);
        Task<User> UpdateUser(Guid userId, User user);
        Task<User> DeleteUser(Guid userId);
        //Roles
        Task<Role> CreateRole(Role role);
        Task<List<Role>> GetUserRoles(Guid userId);
        Task<Role> GetRoleById(Guid roleId);
        Task<Role> ChangeRole(Guid roleId, UserRoles role);
        Task<Role> DeleteRole(Guid roleId);
        //Chats
        Task<List<Chat>> GetUserChats(Guid UserId, int page);
        Task<Chat> GetChatById(Guid chatId);
        Task<Chat> GetChatByUsersId(Guid firstUserId, Guid secondUserId);
        Task<Chat> CreateChat(Guid formUserId, Guid toUserId);
        Task<Chat> RefreshChatLastMessageAt(Guid chatId);
        //Messages
        Task<List<Message>> GetChatMessagesById(Guid ChatId, int page);
        Task<Message> AddMessage(Message message);   
        //FollowingOffers
        Task<List<FollowOffer>> GetUserFollowingOffers(Guid userId);
        Task<FollowOffer> GetUserFollowingOfferById(Guid followingOfferId);
        Task<FollowOffer> AddFollowingOffer(FollowOffer followOffer);
        Task<FollowOffer> DeleteFollowingOffer(Guid followOfferId);
    }
}
