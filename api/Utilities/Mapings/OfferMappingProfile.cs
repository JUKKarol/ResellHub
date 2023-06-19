using AutoMapper;
using ResellHub.DTOs.OfferDTOs;
using ResellHub.Entities;

namespace ResellHub.Utilities.Mapings
{
    public class OfferMappingProfile : Profile
    {
        public OfferMappingProfile()
        {
            CreateMap<OfferPublicDto, Offer>();
            CreateMap<Offer, OfferPublicDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CategoryId));

            CreateMap<OfferCreateDto, Offer>().ReverseMap();
    }
}
}
