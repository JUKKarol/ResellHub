using AutoMapper;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Services.UserServices;

namespace ResellHub.Utilities.Mapings
{
    public class UserMappingProfile : Profile
    {
        private readonly IUserService _userService;

        public UserMappingProfile(IUserService userService)
        {
            _userService = userService;

            CreateMap<UserRegistrationDto, User>()
              .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
              .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
              .ForMember(dest => dest.VeryficationToken, opt => opt.MapFrom(src => _userService.CreateRandomToken()))
              .AfterMap((src, dest, ctx) =>
              {
                  byte[] passwordHash, passwordSalt;
                  _userService.CreatePasswordHash(src.Password, out passwordHash, out passwordSalt);
                  dest.PasswordHash = passwordHash;
                  dest.PasswordSalt = passwordSalt;
              });
        }
    }
}
