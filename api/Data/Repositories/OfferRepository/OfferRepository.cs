using Microsoft.AspNetCore.Mvc.RazorPages;
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

        //Offer
        public async Task<List<Offer>> GetOffers(int page, int pageSize)
        {
            return await _dbContext.Offers
                .OrderBy(o => o.CreatedDate)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetUserOffers(Guid userId, int page, int pageSize)
        {
            return await _dbContext.Offers
                .Where(o => o.UserId == userId)
                .OrderBy(o => o.CreatedDate)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Offer> GetOfferById(Guid offerId)
        {
            var existOffer = await _dbContext.Offers.FirstOrDefaultAsync(u => u.Id == offerId);

            return existOffer;
        }

        public async Task<Offer> GetOfferBySlug(string offerSlug)
        {
            var existOffer = await _dbContext.Offers.FirstOrDefaultAsync(o => o.Slug == offerSlug);

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

        //Category
        public async Task<string> GetCategoryNameById(int categoryId)
        {
            var offerCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            return offerCategory.CategoryName;
        }

        public async Task<List<Category>> GetCategories()
        {
            var categories = await _dbContext.Categories.ToListAsync();

            return categories;
        }
    }
}
