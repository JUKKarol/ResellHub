using ResellHub.Entities;

namespace ResellHub.Data.Repositories.OfferRepository
{
    public interface IOfferRepository
    {
        Task<List<Offer>> GetOffers();
        Task<List<Offer>> GetUserOffers(Guid userId);
        Task<Offer> GetOfferById(Guid offerId);
        Task<Offer> GetOfferBySlug(string offerSlug);
        Task<Offer> AddOffer(Offer offer);
        Task<Offer> UpdateOffer(Guid offerId, Offer offer);
        Task<Offer> DeleteOffer(Guid offerId);
    }
}
