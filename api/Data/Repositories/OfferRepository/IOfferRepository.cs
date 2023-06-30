using ResellHub.Entities;

namespace ResellHub.Data.Repositories.OfferRepository
{
    public interface IOfferRepository
    {
        //Offer
        Task<List<Offer>> GetOffers(int page, int pageSize, Guid loggedUserId);
        Task<List<Offer>> GetUserOffers(string userSlug, int page, int pageSize);
        Task<Offer> GetOfferById(Guid offerId);
        Task<Offer> GetOfferBySlug(string offerSlug);
        Task<Offer> AddOffer(Offer offer);
        Task<Offer> UpdateOffer(Guid offerId, Offer offer);
        Task<Offer> DeleteOffer(Guid offerId);

        //Category
        Task<string> GetCategoryNameById(int categoryId);
        Task<List<Category>> GetCategories();
    }
}
