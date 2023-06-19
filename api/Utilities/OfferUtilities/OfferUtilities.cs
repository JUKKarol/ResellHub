using ResellHub.Data.Repositories.OfferRepository;
using ResellHub.DTOs.OfferDTOs;

namespace ResellHub.Utilities.OfferUtilities
{
    public class OfferUtilities : IOfferUtilities
    {
        private readonly IOfferRepository _offerRepository;

        public OfferUtilities(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<List<OfferPublicDto>> ChangeCategoryIdToCategoryName(List<OfferPublicDto> offerPublicDtos)
        {
            var categories = await _offerRepository.GetCategories();

            foreach (var offerDto in offerPublicDtos)
            {
                offerDto.Category = categories.FirstOrDefault(c => c.Id == int.Parse(offerDto.Category)).CategoryName;
            }

            return offerPublicDtos;
        }

        public async Task<OfferPublicDto> ChangeCategoryIdToCategoryName(OfferPublicDto offerPublicDto)
        {
            var categories = await _offerRepository.GetCategories();

            offerPublicDto.Category = categories.FirstOrDefault(c => c.Id == int.Parse(offerPublicDto.Category)).CategoryName;

            return offerPublicDto;
        }
    }
}
