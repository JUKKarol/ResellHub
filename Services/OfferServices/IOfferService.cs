using ResellHub.DTOs.OfferDTOs;

namespace ResellHub.Services.OfferServices
{
    public interface IOfferService
    {
        Task<List<OfferPublicDto>> GetOffers();
        Task<List<OfferPublicDto>> GetUserOffers(Guid userId);
        Task<OfferPublicDto> GetOfferById(Guid offerId);
        Task<bool> CheckIsOfferExistById(Guid offerId);
        Task<string> AddOffer(OfferCreateDto offerDto, string userEmail, string userEmail);
        Task<string> UpdateOffer(Guid offerId, OfferCreateDto offerDto);
        Task<string> DeleteOffer(Guid offerId);
    }
}
