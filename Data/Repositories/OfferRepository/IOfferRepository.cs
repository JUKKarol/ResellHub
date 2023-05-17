using ResellHub.Entities;

namespace ResellHub.Data.Repositories.OfferRepository
{
    public interface IOfferRepository
    {
        Task<List<Offer>> GetOffer();
        Task<Offer> GetOfferById(Guid offerId);
        Task<Offer> AddOffer(Offer offer);
        Task<Offer> UpdateOffer(Guid offerId, Offer offer);
        Task<Offer> DeleteOffer(Guid offerId);
    }
}
