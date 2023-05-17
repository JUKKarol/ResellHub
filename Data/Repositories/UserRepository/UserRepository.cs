using Microsoft.EntityFrameworkCore;
using ResellHub.Entities;

namespace ResellHub.Data.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ResellHubContext _dbContext;

        public UserRepository(ResellHubContext dbContext)
        {
            _dbContext = dbContext;
        }

        //User
        public async Task<List<User>> GetUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserById(Guid userId)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return existUser;
        }

        public async Task<User> AddUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateUser(Guid userId, User user)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            existUser = user;
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> DeleteUser(Guid userId)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            _dbContext.Users.Remove(existUser);
            await _dbContext.SaveChangesAsync();

            return existUser;
        }

        //Roles
        public async Task<Role> CreateRole(Role role)
        {
            await _dbContext.Roles.AddAsync(role);

            return role;
        }

        public async Task<List<Role>> GetUserRoles(Guid userId)
        {
            var userRoles = await _dbContext.Roles.Where(r => r.UserId == userId).ToListAsync();

            return userRoles;
        }

        public async Task<Role> DeleteRole(Guid roleId)
        {
            var roleToDelete = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            _dbContext.Roles.Remove(roleToDelete);

            return roleToDelete;
        }

        //Messages
        public async Task<List<Message>> GetMessagesBetweenTwoUsers(Guid firstUserId, Guid secondUserId)
        {
            var usersMessages = await _dbContext.Messages
                .Where(m => (m.ToUserId == firstUserId && m.FromUserId == secondUserId) || (m.ToUserId == secondUserId && m.FromUserId == firstUserId))
                .ToListAsync();

            return usersMessages;
        }

        public async Task<Message> AddMessage(Message message)
        {
            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();

            return message;
        }

        //FollowingOffers
        public async Task<List<FollowOffer>> GetUserFollowingOffers(Guid userId)
        {
            var userFolowingOffers = await _dbContext.FollowingOffers.Where(fo => fo.UserId == userId) .ToListAsync();

            return userFolowingOffers;
        }

        public async Task<FollowOffer> AddFollowingOffer(FollowOffer followOffer)
        {
            await _dbContext.FollowingOffers.AddAsync(followOffer);
            await _dbContext.SaveChangesAsync();

            return followOffer;
        }

        public async Task<FollowOffer> DeleteFollowingOffer(Guid folowOfferId)
        {
            var existingFollowOffer = await _dbContext.FollowingOffers.FirstOrDefaultAsync(u => u.Id == folowOfferId);

            _dbContext.FollowingOffers.Remove(existingFollowOffer);
            await _dbContext.SaveChangesAsync();

            return existingFollowOffer;
        }
    }
}
