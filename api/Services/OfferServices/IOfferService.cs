using ResellHub.DTOs.OfferDTOs;
using ResellHub.DTOs.SharedDTOs;
using Sieve.Models;

namespace ResellHub.Services.OfferServices
{
    public interface IOfferService
    {
        //Offer
        Task<PagedRespondListDto<OfferPublicDto>> GetOffers(SieveModel query, Guid loggedUserId);

        Task<PagedRespondListDto<OfferPublicDto>> GetUserOffers(string userSlug, SieveModel query, Guid loggedUserId);

        Task<OfferDetalisDto> GetOfferById(Guid offerId, Guid loggedUserId);

        Task<OfferDetalisDto> GetOfferBySlug(string offerSlug, Guid loggedUserId);

        Task<OfferDetalisDto> GetOfferByOfferImageSlug(string offerImageSlug);

        Task<Guid> GetOfferIdByOfferImageSlug(string offerImageSlug);

        Task<Guid> GetOfferIdByOfferSlug(string offerSlug);

        Task<bool> CheckIsOfferExistById(Guid offerId);

        Task<bool> CheckIsOfferOwnerCorrectByEmail(Guid offerId, string userEmail);

        Task<bool> AddOffer(OfferCreateDto offerDto, Guid userId);

        Task UpdateOffer(string offerSlug, OfferUpdateDto offerDto);

        Task DeleteOffer(string offerSlug);

        //Image
        Task<string> SetOfferImageAsPrimaryBySlug(string offerImageSlug);
    }
}