using ResellHub.Data.Repositories.OfferRepository;
using ResellHub.DTOs.OfferDTOs;

namespace ResellHub.Utilities.OfferUtilities
{
    public interface IOfferUtilities
    {
        Task<List<OfferPublicDto>> ChangeCategoryIdToCategoryName(List<OfferPublicDto> offerPublicDtos);
        Task<List<OfferDetalisDto>> ChangeCategoryIdToCategoryName(List<OfferDetalisDto> offerDetalisDtos);
        Task<OfferPublicDto> ChangeCategoryIdToCategoryName(OfferPublicDto offerPublicDto);
        Task<OfferDetalisDto> ChangeCategoryIdToCategoryName(OfferDetalisDto offerDetalisDto);
    }
}
