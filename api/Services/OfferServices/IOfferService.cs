using ResellHub.DTOs.OfferDTOs;

namespace ResellHub.Services.OfferServices
{
    public interface IOfferService
    {
        Task<List<OfferPublicDto>> GetOffers(int page);
        Task<List<OfferPublicDto>> GetUserOffers(string userSlug, int page);
        Task<OfferDetalisDto> GetOfferById(Guid offerId);
        Task<OfferDetalisDto> GetOfferBySlug(string slug);
        Task<bool> CheckIsOfferExistById(Guid offerId);
        Task<bool> CheckIsOfferOwnerCorrectByEmail(Guid offerId, string userEmail);
        Task<string> AddOffer(OfferCreateDto offerDto, string userEmail);
        Task<string> UpdateOffer(Guid offerId, OfferCreateDto offerDto);
        Task<string> DeleteOffer(Guid offerId);
    }
}
