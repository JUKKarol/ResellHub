using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;

namespace ResellHub.Services.UserServices
{
    public interface IUserService
    {
        Task<List<UserPublicDto>> GetUsers();
        Task<UserPublicDto> GetUserById(Guid userId);
        Task<string> CreateUser(UserRegistrationDto user);
        Task<string> UpdatePhoneNumber(Guid userId, string phoneNumber);
        Task<string> UpdateCity(Guid userId, string city);
        Task<string> DeleteUser(Guid userId);
    }
}
