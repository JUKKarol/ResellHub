using ResellHub.DTOs.OfferDTOs;
using ResellHub.Entities;

namespace ResellHub.Services.OfferServices
{
    public interface IOfferService
    {
        Task<List<OfferPublicDto>> GetOffers(int page, Guid loggedUserId);
        Task<List<OfferPublicDto>> GetUserOffers(string userSlug, int page, Guid loggedUserId);
        Task<OfferDetalisDto> GetOfferById(Guid offerId, Guid loggedUserId);
        Task<OfferDetalisDto> GetOfferBySlug(string offerSlug, Guid loggedUserId);
        Task<Offer> GetOfferByOfferImageSlug(string offerImageSlug);
        Task<Guid> GetOfferIdByOfferImageSlug(string offerImageSlug);
        Task<Guid> GetOfferIdByOfferSlug(string offerSlug);
        Task<bool> CheckIsOfferExistById(Guid offerId);
        Task<bool> CheckIsOfferOwnerCorrectByEmail(Guid offerId, string userEmail);
        Task<string> AddOffer(OfferCreateDto offerDto, string userEmail);
        Task<string> SetOfferImageAsPrimary(OfferCreateDto offerDto, string userEmail);
        Task<string> SetOfferImageAsPrimaryBySlug(string offerImageSlug);
        Task<string> UpdateOffer(Guid offerId, OfferCreateDto offerDto);
        Task<string> DeleteOffer(Guid offerId);
    }
}
