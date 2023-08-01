using AutoMapper;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Services.UserServices;

namespace ResellHub.Utilities.Mapings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserRegistrationDto, User>().ReverseMap();
            CreateMap<UserLoginDto, User>().ReverseMap();
            CreateMap<UserPublicDto, User>().ReverseMap();
            CreateMap<UserDetalisDto, User>().ReverseMap();
        }
    }
}
