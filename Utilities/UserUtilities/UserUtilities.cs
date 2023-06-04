using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ResellHub.Data.Repositories.UserRepository;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ResellHub.Utilities.UserUtilities
{
    public class UserUtilities : IUserUtilities
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserUtilities(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<string> CreateToken(UserLoginDto userDto)
        {
            var user = await _userRepository.GetUserByEmail(userDto.Email);
            var userRoles = await _userRepository.GetUserRoles(user.Id);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userDto.Email),
            };

            if (userRoles.Any(ur => ur.UserRole == UserRoles.Moderator || ur.UserRole == UserRoles.Administrator || ur.UserRole == UserRoles.User))
            {
                claims.Add(new Claim(ClaimTypes.Role, UserRoles.User.ToString()));
            }

            if (userRoles.Any(ur => ur.UserRole == UserRoles.Moderator))
            {
                claims.Add(new Claim(ClaimTypes.Role, UserRoles.Moderator.ToString()));
            }

            if (userRoles.Any(ur => ur.UserRole == UserRoles.Administrator))
            {
                claims.Add(new Claim(ClaimTypes.Role, UserRoles.Moderator.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, UserRoles.Administrator.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
