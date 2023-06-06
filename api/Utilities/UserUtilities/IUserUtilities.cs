using ResellHub.DTOs.UserDTOs;

namespace ResellHub.Utilities.UserUtilities
{
    public interface IUserUtilities
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateRandomToken();
        Task<string> CreateToken(UserLoginDto userDto);
    }
}
