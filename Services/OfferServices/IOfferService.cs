using ResellHub.DTOs.OfferDTOs;

namespace ResellHub.Services.OfferServices
{
    public interface IOfferService
    {
        Task<List<OfferPublicDto>> GetOffers();
        Task<dynamic> GetUserOffers(Guid userId);
        Task<dynamic> GetOfferById(Guid offerId);
        Task<bool> CheckIsOfferExistById(Guid offerId);
        Task<string> AddOffer(OfferCreateDto offerDto);
        Task<string> UpdateOffer(Guid offerId, OfferCreateDto offerDto);
        Task<string> DeleteOffer(Guid offerId);
    }
}
