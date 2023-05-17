using Microsoft.EntityFrameworkCore;
using ResellHub.Entities;

namespace ResellHub.Data.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ResellHubContext _dbContext;

        public UserRepository(ResellHubContext resellHub)
        {
            _dbContext = resellHub;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<bool> ChcekIsUserWithEmailExist(string email)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (existUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
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
            List<Role> userRoles = await _dbContext.Roles.Where(r => r.UserId == userId).ToListAsync();

            return userRoles;
        }

        public async Task<Role> DeleteRole(Guid roleId)
        {
            var roleToDelete = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            _dbContext.Roles.Remove(roleToDelete);

            return roleToDelete;
        }

        //Messages


        //FollowingOffers
    }
}
