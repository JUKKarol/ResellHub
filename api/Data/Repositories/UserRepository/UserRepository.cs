using Microsoft.EntityFrameworkCore;
using ResellHub.Entities;
using ResellHub.Enums;
using System.Data;

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
        public async Task<List<User>> GetUsers(int page, int pageSize)
        {
            return await _dbContext.Users
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> GetUsersCount()
        {
            return await _dbContext.Users.CountAsync();
        }

        public async Task<User> GetUserById(Guid userId)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return existUser;
        }

        public async Task<User> GetUserByEmail(string userEmail)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == userEmail);
        }

        public async Task<User> GetUserByVeryficationToken(string userToken)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.VeryficationToken == userToken);
        }

        public async Task<User> GetUserByResetToken(string userToken)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.PasswordResetToken == userToken);
        }

        public async Task<User> GetUserBySlug(string userSlug)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Slug == userSlug);

            return existUser;
        }
        public async Task<User> GetUserBySlugIncludeAvatar(string userSlug)
        {
            return await _dbContext.Users
                .Include(u => u.AvatarImage)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Slug == userSlug);
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
            await _dbContext.SaveChangesAsync();

            return role;
        }

        public async Task<List<Role>> GetUserRoles(Guid userId)
        {
            return await _dbContext.Roles
                .Where(r => r.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Role> GetRoleById(Guid roleId)
        {
            return await _dbContext.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == roleId);
        }

        public async Task<Role> ChangeRole(Guid roleId, UserRoles role)
        {
            var existRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            existRole.UserRole = role;
            await _dbContext.SaveChangesAsync();

            return existRole;
        }

        public async Task<Role> DeleteRole(Guid roleId)
        {
            var roleToDelete = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            _dbContext.Roles.Remove(roleToDelete);
            await _dbContext.SaveChangesAsync();

            return roleToDelete;
        }

        //Chats
        public async Task<List<Chat>> GetUserChats(Guid userId, int page, int pageSize)
        {
            return await _dbContext.Chats
                .Where(c => (c.SenderId == userId) || (c.ReciverId == userId))
                .OrderBy(c => c.LastMessageAt)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> GetUserChatsCount(Guid userId)
        {
            return await _dbContext.Chats
                .Where(c => (c.SenderId == userId) || (c.ReciverId == userId))
                .CountAsync();
        }

        public async Task<Chat> GetChatById(Guid chatId)
        {
            return await _dbContext.Chats
                .AsNoTracking()
                .FirstOrDefaultAsync(c => (c.Id == chatId));
        }

        public async Task<Chat> GetChatByUsersId(Guid firstUserId, Guid secondUserId)
        {
            return await _dbContext.Chats
                .AsNoTracking()
                .FirstOrDefaultAsync(c => (c.SenderId == firstUserId) || (c.ReciverId == firstUserId) && (c.SenderId == secondUserId) || (c.ReciverId == secondUserId));
        }

        public async Task<Chat> CreateChat(Guid formUserId, Guid reciverId)
        {
            var chat = new Chat()
            {
                SenderId = formUserId,
                ReciverId = reciverId,
            };

            await _dbContext.Chats.AddAsync(chat);
            await _dbContext.SaveChangesAsync();

            return chat;
        }

        public async Task<Chat> RefreshChatLastMessageAt(Guid chatId)
        {
            var chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
            chat.LastMessageAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return chat;
        }

        //Messages
        public async Task<List<Message>> GetChatMessagesById(Guid ChatId, int page, int pageSize)
        {
            return await _dbContext.Messages
                .Where(m => m.ChatId == ChatId)
                .OrderBy(c => c.CreatedDate)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> GetMessagesInChatCount(Guid ChatId)
        {
            return await _dbContext.Messages
                .Where(m => m.ChatId == ChatId)
                .CountAsync();
        }

        public async Task<Message> AddMessage(Message message)
        {
            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();

            return message;
        }

        //FollowingOffers
        public async Task<List<FollowOffer>> GetUserFollowingOffers(Guid userId, int page, int pageSize)
        {
            return await _dbContext.FollowingOffers
                .Where(fo => fo.UserId == userId)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> GetUserFollowingOffersCount(Guid userId)
        {
            return await _dbContext.FollowingOffers.Where(fo => fo.UserId == userId).CountAsync();
        }

        public async Task<FollowOffer> GetUserFollowingOfferById(Guid followingOfferId)
        {
            return await _dbContext.FollowingOffers
                .AsNoTracking()
                .FirstOrDefaultAsync(fo => fo.Id == followingOfferId);
        }

        public async Task<FollowOffer> GetFollowingOfferByUserAndOfferId(Guid followingOfferId, Guid userId)
        {
            return await _dbContext.FollowingOffers
                .AsNoTracking()
                .FirstOrDefaultAsync(fo => (fo.Id == followingOfferId) && (fo.UserId == userId));
        }

        public async Task<FollowOffer> AddFollowingOffer(FollowOffer followOffer)
        {
            await _dbContext.FollowingOffers.AddAsync(followOffer);
            await _dbContext.SaveChangesAsync();

            return followOffer;
        }

        public async Task<FollowOffer> DeleteFollowingOffer(Guid followOfferId)
        {
            var existingFollowOffer = await _dbContext.FollowingOffers.FirstOrDefaultAsync(u => u.Id == followOfferId);

            _dbContext.FollowingOffers.Remove(existingFollowOffer);
            await _dbContext.SaveChangesAsync();

            return existingFollowOffer;
        }

        //AvatarImage
        public async Task<AvatarImage> AddAvatarImage(AvatarImage avatarImage)
        {
            await _dbContext.AvatarImages.AddAsync(avatarImage);
            await _dbContext.SaveChangesAsync();

            return avatarImage;
        }

        public async Task<AvatarImage> GetAvatarImageByUserId(Guid userId)
        {
            return await _dbContext.AvatarImages
                .AsNoTracking()
                .FirstOrDefaultAsync(ai => ai.UserId == userId);
        }

        public async Task<AvatarImage> GetAvatarImageBySlug(string slug)
        {
            return await _dbContext.AvatarImages
                .AsNoTracking()
                .FirstOrDefaultAsync(ai => ai.ImageSlug == slug);
        }

        public async Task<AvatarImage> DeleteAvatarImage(Guid userId)
        {
            var existingAvatarImage = await _dbContext.AvatarImages.FirstOrDefaultAsync(ai => ai.UserId == userId);

            _dbContext.AvatarImages.Remove(existingAvatarImage);
            await _dbContext.SaveChangesAsync();

            return existingAvatarImage;
        }
    }
}
