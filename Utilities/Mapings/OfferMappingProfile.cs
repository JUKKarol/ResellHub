using AutoMapper;
using ResellHub.DTOs.OfferDTOs;
using ResellHub.Entities;

namespace ResellHub.Utilities.Mapings
{
    public class OfferMappingProfile : Profile
    {
        public OfferMappingProfile()
        {
            CreateMap<OfferPublicDto, Offer>().ReverseMap();
            CreateMap<OfferCreateDto, Offer>().ReverseMap();
        }
    }
}
