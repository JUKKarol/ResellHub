using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;

namespace ResellHub.Services.UserServices
{
    public interface IUserService
    {
        Task<string> CreateUser(UserRegistrationDto user);
        Task<List<UserPublicDto>> GetUsers();
    }
}
