using ResellHub.Data.Repositories.OfferRepository;
using ResellHub.DTOs.OfferDTOs;

namespace ResellHub.Utilities.OfferUtilities
{
    public interface IOfferUtilities
    {
        Task<List<OfferPublicDto>> ChangeCategoryIdToCategoryName(List<OfferPublicDto> offerPublicDtos);
        Task<OfferPublicDto> ChangeCategoryIdToCategoryName(OfferPublicDto offerPublicDto);
    }
}
