using ResellHub.Entities;

namespace ResellHub.Data.Repositories.OfferRepository
{
    public interface IOfferRepository
    {
        //Offer
        Task<List<Offer>> GetOffers(int page);
        Task<List<Offer>> GetUserOffers(Guid userId, int page);
        Task<Offer> GetOfferById(Guid offerId);
        Task<Offer> GetOfferBySlug(string offerSlug);
        Task<Offer> AddOffer(Offer offer);
        Task<Offer> UpdateOffer(Guid offerId, Offer offer);
        Task<Offer> DeleteOffer(Guid offerId);

        //Category
        Task<String> GetCategoryNameById(int categoryId);
        Task<List<Category>> GetCategories();
    }
}
