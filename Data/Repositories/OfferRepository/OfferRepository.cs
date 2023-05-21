using Microsoft.EntityFrameworkCore;
using ResellHub.Entities;

namespace ResellHub.Data.Repositories.OfferRepository
{
    public class OfferRepository : IOfferRepository
    {
        private readonly ResellHubContext _dbContext;

        public OfferRepository(ResellHubContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Offer>> GetOffers()
        {
            return await _dbContext.Offers.ToListAsync();
        }

        public async Task<List<Offer>> GetUserOffers(Guid userId)
        {
            return await _dbContext.Offers.Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<Offer> GetOfferById(Guid offerId)
        {
            var existOffer = await _dbContext.Offers.FirstOrDefaultAsync(u => u.Id == offerId);

            return existOffer;
        }

        public async Task<Offer> GetOfferByEncodedName(string offerEncodedName)
        {
            var existOffer = await _dbContext.Offers.FirstOrDefaultAsync(o => o.EncodedName == offerEncodedName);

            return existOffer;
        }

        public async Task<Offer> AddOffer(Offer offer)
        {
            await _dbContext.Offers.AddAsync(offer);
            await _dbContext.SaveChangesAsync();

            return offer;
        }

        public async Task<Offer> UpdateOffer(Guid offerId, Offer offer)
        {
            var existOffer = await _dbContext.Offers.FirstOrDefaultAsync(u => u.Id == offerId);

            existOffer = offer;
            await _dbContext.SaveChangesAsync();

            return offer;
        }

        public async Task<Offer> DeleteOffer(Guid offerId)
        {
            var existOffer = await _dbContext.Offers.FirstOrDefaultAsync(u => u.Id == offerId);

            _dbContext.Offers.Remove(existOffer);
            await _dbContext.SaveChangesAsync();

            return existOffer;
        }
    }
}
