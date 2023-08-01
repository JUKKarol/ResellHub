using ResellHub.DTOs.OfferDTOs;
using ResellHub.Entities;

namespace ResellHub.Services.OfferServices
{
    public interface IOfferService
    {
        //Offer
        Task<List<OfferPublicDto>> GetOffers(int page, Guid loggedUserId);
        Task<List<OfferPublicDto>> GetUserOffers(string userSlug, int page, Guid loggedUserId);
        Task<OfferDetalisDto> GetOfferById(Guid offerId, Guid loggedUserId);
        Task<OfferDetalisDto> GetOfferBySlug(string offerSlug, Guid loggedUserId);
        Task<OfferDetalisDto> GetOfferByOfferImageSlug(string offerImageSlug);
        Task<Guid> GetOfferIdByOfferImageSlug(string offerImageSlug);
        Task<Guid> GetOfferIdByOfferSlug(string offerSlug);
        Task<bool> CheckIsOfferExistById(Guid offerId);
        Task<bool> CheckIsOfferOwnerCorrectByEmail(Guid offerId, string userEmail);
        Task<string> AddOffer(OfferCreateDto offerDto, string userEmail);
        Task<string> UpdateOffer(Guid offerId, OfferUpdateDto offerDto);
        Task<string> DeleteOffer(Guid offerId);
        //Image
        Task<string> SetOfferImageAsPrimaryBySlug(string offerImageSlug);
    }
}
