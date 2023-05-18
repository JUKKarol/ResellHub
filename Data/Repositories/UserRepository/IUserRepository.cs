using ResellHub.Entities;

namespace ResellHub.Data.Repositories.UserRepository
{
    public interface IUserRepository
    {
        //User
        Task<List<User>> GetUsers();
        Task<User> GetUserById(Guid userId);
        Task<User> GetUserByEmail(string userEmail);
        Task<User> AddUser(User user);
        Task<User> UpdateUser(Guid userId, User user);
        Task<User> DeleteUser(Guid userId);
        //Roles
        Task<Role> CreateRole(Role role);
        Task<List<Role>> GetUserRoles(Guid userId);
        Task<Role> DeleteRole(Guid roleId);
        //Messages
        Task<List<Message>> GetMessagesBetweenTwoUsers(Guid firstUserId, Guid secondUserId);
        Task<Message> AddMessage(Message message);
        //FollowingOffers
        Task<List<FollowOffer>> GetUserFollowingOffers(Guid userId);
        Task<FollowOffer> AddFollowingOffer(FollowOffer followOffer);
        Task<FollowOffer> DeleteFollowingOffer(Guid followOfferId);
    }
}
