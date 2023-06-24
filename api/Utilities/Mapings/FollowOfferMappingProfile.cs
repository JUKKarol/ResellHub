using AutoMapper;
using ResellHub.DTOs.FollowOfferDTOs;
using ResellHub.Entities;

namespace ResellHub.Utilities.Mapings
{
    public class FollowOfferMappingProfile : Profile
    {
        public FollowOfferMappingProfile()
        {
            CreateMap<FollowOfferDto, FollowOffer>().ReverseMap();
        }
    }
}
