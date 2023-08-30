using AutoMapper;
using ResellHub.DTOs.OfferImageDTOs;
using ResellHub.Entities;

namespace ResellHub.Utilities.Mapings
{
    public class OfferImageMappingProfile : Profile
    {
        public OfferImageMappingProfile()
        {
            CreateMap<OfferImageDisplayDTO, OfferImage>().ReverseMap();
        }
    }
}