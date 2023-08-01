using ResellHub.Entities;

namespace ResellHub.Data.Repositories.OfferRepository
{
    public interface IOfferRepository
    {
        //Offer
        Task<List<Offer>> GetOffers(int page, int pageSize);
        Task<List<Offer>> GetUserOffers(string userSlug, int page, int pageSize);
        Task<Offer> GetOfferById(Guid offerId);
        Task<Offer> GetOfferBySlug(string offerSlug);
        Task<Offer> AddOffer(Offer offer);
        Task<Offer> UpdateOffer(Guid offerId, Offer offer);
        Task<Offer> DeleteOffer(Guid offerId);

        //Category
        Task<string> GetCategoryNameById(int categoryId);
        Task<List<Category>> GetCategories();
        //OfferImages
        Task<OfferImage> AddOfferImageImage(OfferImage offerImage);
        Task<OfferImage> GetOfferImageById(Guid offerImageId);
        Task<List<OfferImage>> GetAllOfferImagesByOfferId(Guid offerId);
        Task<List<OfferImage>> GetAllOfferImagesByOfferslug(string offerSlug);
        Task<OfferImage> GetPrimaryOfferImageByOfferId(Guid offerId);
        Task<OfferImage> GetOfferImageBySlug(string offerImageSlug);
        Task<OfferImage> SetOfferImageAsPrimaryBySlug(string offerImageSlug);
        Task<OfferImage> DeleteOfferImage(string offerImageSlug);
    }
}
