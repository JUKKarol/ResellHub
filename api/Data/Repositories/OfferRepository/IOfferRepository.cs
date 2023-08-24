using ResellHub.Entities;
using Sieve.Models;

namespace ResellHub.Data.Repositories.OfferRepository
{
    public interface IOfferRepository
    {
        //Offer
        Task<List<Offer>> GetOffers(SieveModel query);
        Task<int> GetOffersCount();
        Task<List<Offer>> GetUserOffers(string userSlug, int page, int pageSize);
        Task<Offer> GetOfferById(Guid offerId);
        Task<Offer> GetOfferBySlug(string offerSlug);
        Task<Offer> AddOffer(Offer offer);
        Task<Offer> UpdateOffer(string offerSlug, Offer offer);
        Task<Offer> DeleteOffer(string offerSlug);

        //Category
        Task<string> GetCategoryNameById(int categoryId);
        Task<List<Category>> GetCategories();
        //OfferImages
        Task<OfferImage> AddOfferImageImage(OfferImage offerImage);
        Task<OfferImage> GetOfferImageById(Guid offerImageId);
        Task<List<OfferImage>> GetAllOfferImagesByOfferId(Guid offerId);
        Task<List<OfferImage>> GetAllOfferImagesByOfferSlug(string offerSlug);
        Task<OfferImage> GetPrimaryOfferImageByOfferId(Guid offerId);
        Task<OfferImage> GetOfferImageBySlug(string offerImageSlug);
        Task<OfferImage> SetOfferImageAsPrimaryBySlug(string offerImageSlug);
        Task<OfferImage> DeleteOfferImage(string offerImageSlug);
    }
}
