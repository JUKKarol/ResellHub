using AutoMapper;
using ResellHub.DTOs.RoleDTOs;
using ResellHub.Entities;

namespace ResellHub.Utilities.Mapings
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<RoleDto, Role>().ReverseMap();
        }
    }
}
