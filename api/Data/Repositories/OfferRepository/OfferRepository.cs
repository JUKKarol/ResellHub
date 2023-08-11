﻿using Microsoft.AspNetCore.Mvc.RazorPages;
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
                .Include(o => o.FollowingOffers)
                .Include(o => o.OfferImages)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Offer>> GetUserOffers(string userSlug, int page, int pageSize)
        {
            return await _dbContext.Offers
                .Where(o => o.User.Slug == userSlug)
                .OrderBy(o => o.CreatedDate)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .Include(o => o.FollowingOffers)
                .Include(o => o.OfferImages)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Offer> GetOfferById(Guid offerId)
        {
            return await _dbContext.Offers
                .Include(o => o.FollowingOffers)
                .Include(o => o.OfferImages)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == offerId);
        }

        public async Task<Offer> GetOfferBySlug(string offerSlug)
        {
            var existOffer = await _dbContext.Offers
                .Include(o => o.FollowingOffers)
                .Include(o => o.OfferImages)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Slug == offerSlug);

            return existOffer;
        }

        public async Task<Offer> AddOffer(Offer offer)
        {
            await _dbContext.Offers.AddAsync(offer);
            await _dbContext.SaveChangesAsync();

            return offer;
        }

        public async Task<Offer> UpdateOffer(string offerSlug, Offer offer)
        {
            var existOffer = await _dbContext.Offers.FirstOrDefaultAsync(u => u.Slug == offerSlug);

            existOffer = offer;
            await _dbContext.SaveChangesAsync();

            return offer;
        }

        public async Task<Offer> DeleteOffer(string offerSlug)
        {
            var existOffer = await _dbContext.Offers.FirstOrDefaultAsync(u => u.Slug == offerSlug);

            _dbContext.Offers.Remove(existOffer);
            await _dbContext.SaveChangesAsync();

            return existOffer;
        }

        //Category
        public async Task<string> GetCategoryNameById(int categoryId)
        {
            var offerCategory = await _dbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            return offerCategory.CategoryName;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        //OfferImages
        public async Task<OfferImage> AddOfferImageImage(OfferImage offerImage)
        {
            await _dbContext.OfferImages.AddAsync(offerImage);
            await _dbContext.SaveChangesAsync();

            return offerImage;
        }

        public async Task<OfferImage> GetOfferImageById(Guid offerImageId)
        {
            return await _dbContext.OfferImages
                .AsNoTracking()
                .FirstOrDefaultAsync(oi => oi.Id == offerImageId);
        }

        public async Task<List<OfferImage>> GetAllOfferImagesByOfferId(Guid offerId)
        {
            return await _dbContext.OfferImages
                .Where(oi => oi.OfferId == offerId)
                .OrderByDescending(oi => oi.CreatedDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<OfferImage>> GetAllOfferImagesByOfferslug(string offerSlug)
        {
            return await _dbContext.OfferImages
                .Where(oi => oi.Offer.Slug == offerSlug)
                .OrderByDescending(oi => oi.CreatedDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<OfferImage> GetPrimaryOfferImageByOfferId(Guid offerId)
        {
            return await _dbContext.OfferImages
                .OrderByDescending(oi => oi.CreatedDate)
                .AsNoTracking()
                .FirstOrDefaultAsync(oi => oi.OfferId == offerId);
        }

        public async Task<OfferImage> GetOfferImageBySlug(string offerImageSlug)
        {
            return await _dbContext.OfferImages
                .AsNoTracking()
                .FirstOrDefaultAsync(oi => oi.ImageSlug == offerImageSlug);
        }

        public async Task<OfferImage> SetOfferImageAsPrimaryBySlug(string offerImageSlug)
        {
            var offerImage = await _dbContext.OfferImages.FirstOrDefaultAsync(oi => oi.ImageSlug == offerImageSlug);
            offerImage.CreatedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return offerImage;
        }

        public async Task<OfferImage> DeleteOfferImage(string offerImageSlug)
        {
            var existingOfferImage = await _dbContext.OfferImages.FirstOrDefaultAsync(oi => oi.ImageSlug == offerImageSlug);

            _dbContext.OfferImages.Remove(existingOfferImage);
            await _dbContext.SaveChangesAsync();

            return existingOfferImage;
        }
    }
}
