using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ResellHub.Data.Repositories.UserRepository;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Enums;
using ResellHub.Utilities.UserUtilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ResellHub.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserUtilities _userService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IUserUtilities userService, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _userService = userService;
            _configuration = configuration;
            _mapper = mapper;
        }

        //User
        

        public async Task<string> CreateUser(UserRegistrationDto userDto)
        {
            if(await _userRepository.GetUserByEmail(userDto.Email) != null)
            {
                return "User already exists";
            }

            var user = _mapper.Map<User>(userDto);
            user.EncodeName();

            byte[] passwordHash;
            byte[] passwordSalt;
            _userService.CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.VeryficationToken = _userService.CreateRandomToken();

            await _userRepository.AddUser(user);

            var userBasicRole = new Role { UserId = user.Id };
            await _userRepository.CreateRole(userBasicRole);

            return "User ceated successful";
        }

        public async Task<List<UserPublicDto>> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            var usersDto = _mapper.Map<List<UserPublicDto>>(users);

            return usersDto;
        }
    }
}
