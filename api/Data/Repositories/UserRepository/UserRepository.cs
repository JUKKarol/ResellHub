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
                .ToListAsync();
        }

        public async Task<User> GetUserById(Guid userId)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return existUser;
        }

        public async Task<User> GetUserByEmail(string userEmail)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            return existUser;
        }

        public async Task<User> GetUserByVeryficationToken(string userToken)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.VeryficationToken == userToken);

            return existUser;
        }

        public async Task<User> GetUserByResetToken(string userToken)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == userToken);

            return existUser;
        }

        public async Task<User> GetUserBySlug(string userSlug)
        {
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Slug == userSlug);

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
            await _dbContext.SaveChangesAsync();

            return role;
        }

        public async Task<List<Role>> GetUserRoles(Guid userId)
        {
            var userRoles = await _dbContext.Roles.Where(r => r.UserId == userId).ToListAsync();

            return userRoles;
        }

        public async Task<Role> GetRoleById(Guid roleId)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            return role;
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
            var usersChats = await _dbContext.Chats
                .Where(c => (c.SenderId == userId) || (c.ReciverId == userId))
                .OrderBy(c => c.LastMessageAt)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();

            return usersChats;
        }

        public async Task<Chat> GetChatById(Guid chatId)
        {
            var chat = await _dbContext.Chats
                .FirstOrDefaultAsync(c => (c.Id == chatId));

            return chat;
        }

        public async Task<Chat> GetChatByUsersId(Guid firstUserId, Guid secondUserId)
        {
            var usersChats = await _dbContext.Chats
                .FirstOrDefaultAsync(c => (c.SenderId == firstUserId) || (c.ReciverId == firstUserId) && (c.SenderId == secondUserId) || (c.ReciverId == secondUserId));

            return usersChats;
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
            var chatMessages = await _dbContext.Messages
                .Where(m => m.ChatId == ChatId)
                .OrderBy(c => c.CreatedDate)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();

            return chatMessages;
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
            var userFolowingOffers = await _dbContext.FollowingOffers
                .Where(fo => fo.UserId == userId)
                .Skip(pageSize * (page - 1))
                .Take(pageSize).ToListAsync();

            return userFolowingOffers;
        }

        public async Task<FollowOffer> GetUserFollowingOfferById(Guid followingOfferId)
        {
            var followingOffer = await _dbContext.FollowingOffers.FirstOrDefaultAsync(fo => fo.Id == followingOfferId);

            return followingOffer;
        }

        public async Task<FollowOffer> GetFollowingOfferByUserAndOfferId(Guid followingOfferId, Guid userId)
        {
            var followingOffer = await _dbContext.FollowingOffers.FirstOrDefaultAsync(fo => (fo.Id == followingOfferId) && (fo.UserId == userId));

            return followingOffer;
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
        //AvatarImage
        public async Task<AvatarImage> AddAvatarImage(string ImageSlug, Guid UserId)
        {
            var avatarImage = new AvatarImage()
            {
                ImageSlug = ImageSlug,
                UserId = UserId
            };

            await _dbContext.AvatarImages.AddAsync(avatarImage);
            await _dbContext.SaveChangesAsync();

            return avatarImage;
        }

        public async Task<string> GetAvatarImageSlugByUserId(Guid userId)
        {
            var avatarImage = await _dbContext.AvatarImages.FirstOrDefaultAsync(ai => ai.UserId == userId);

            return avatarImage.ImageSlug;
        }

        public async Task<AvatarImage> DeleteAvatarImage(string ImageSlug)
        {
            var existingAvatarImage = await _dbContext.AvatarImages.FirstOrDefaultAsync(ai => ai.ImageSlug == ImageSlug);

            _dbContext.AvatarImages.Remove(existingAvatarImage);
            await _dbContext.SaveChangesAsync();

            return existingAvatarImage;
        }
    }
}
