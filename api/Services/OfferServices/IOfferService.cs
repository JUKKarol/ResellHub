﻿using ResellHub.DTOs.OfferDTOs;

namespace ResellHub.Services.OfferServices
{
    public interface IOfferService
    {
        Task<List<OfferPublicDto>> GetOffers(int page, Guid loggedUserId);
        Task<List<OfferPublicDto>> GetUserOffers(string userSlug, int page, Guid loggedUserId);
        Task<OfferDetalisDto> GetOfferById(Guid offerId, Guid loggedUserId);
        Task<OfferDetalisDto> GetOfferBySlug(string slug, Guid loggedUserId);
        Task<Guid> GetOfferIdBySlug(string offerSlug);
        Task<bool> CheckIsOfferExistById(Guid offerId);
        Task<bool> CheckIsOfferOwnerCorrectByEmail(Guid offerId, string userEmail);
        Task<string> AddOffer(OfferCreateDto offerDto, string userEmail);
        Task UpdateOffer(Guid offerId, OfferCreateDto offerDto);
        Task DeleteOffer(Guid offerId);
    }
}
