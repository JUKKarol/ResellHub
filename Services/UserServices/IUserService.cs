using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;

namespace ResellHub.Services.UserServices
{
    public interface IUserService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateRandomToken();
        Task<string> CreateToken(UserLoginDto userDto);
        Task<string> CreateUser(UserRegistrationDto user);
        Task<List<UserPublicDto>> GetUsers();
    }
}
