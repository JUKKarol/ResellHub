using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResellHub.Data.Repositories.OfferRepository;
using ResellHub.Data.Repositories.UserRepository;
using ResellHub.DTOs.OfferDTOs;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Utilities.OfferUtilities;
using ResellHub.Utilities.UserUtilities;
using System;

namespace ResellHub.Services.OfferServices
{
    public class OfferService : IOfferService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IOfferUtilities _offerUtilities;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IValidator<OfferCreateDto> _offerValidator;

        public OfferService(IUserRepository userRepository, IOfferRepository offerRepository, IOfferUtilities offerUtilities, IConfiguration configuration, IMapper mapper, IValidator<OfferCreateDto> offerValidator)
        {
            _userRepository = userRepository;
            _offerRepository = offerRepository;
            _offerUtilities = offerUtilities;
            _configuration = configuration;
            _mapper = mapper;
            _offerValidator = offerValidator;
        }

        public async Task<List<OfferPublicDto>> GetOffers(int page, Guid loggedUserId)
        {
            var offers = await _offerRepository.GetOffers(page, 40);
            var offersDto = _mapper.Map<List<OfferPublicDto>>(offers);

            var followedOfferSlugs = offers
                .Where(offer => offer.FollowingOffers.Any(fo => fo.UserId == loggedUserId))
                .Select(offer => offer.Slug)
                .ToList();

            foreach (var offerDto in offersDto)
            {
                offerDto.IsUserFollowing = followedOfferSlugs.Contains(offerDto.Slug);
            }

            return await _offerUtilities.ChangeCategoryIdToCategoryName(offersDto);
        }

        public async Task<List<OfferPublicDto>> GetUserOffers(string userSlug, int page)
        {
            var offers = await _offerRepository.GetUserOffers(userSlug, page, 40);
            var offersDto = _mapper.Map<List<OfferPublicDto>>(offers);

            return await _offerUtilities.ChangeCategoryIdToCategoryName(offersDto);
        }

        public async Task<OfferDetalisDto> GetOfferById(Guid offerId)
        {
            var offer = await _offerRepository.GetOfferById(offerId);
            var offerDto = _mapper.Map<OfferDetalisDto>(offer);

            return await _offerUtilities.ChangeCategoryIdToCategoryName(offerDto);
        }

        public async Task<OfferDetalisDto> GetOfferBySlug(string slug)
        {
            var offer = await _offerRepository.GetOfferBySlug(slug);
            var offerDto = _mapper.Map<OfferDetalisDto>(offer);

            return await _offerUtilities.ChangeCategoryIdToCategoryName(offerDto);
        }

        public async Task<bool> CheckIsOfferExistById(Guid offerId)
        {
            var offer = await _offerRepository.GetOfferById(offerId);

            if (offer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> CheckIsOfferOwnerCorrectByEmail(Guid offerId, string userEmail)
        {
            var offer = await _offerRepository.GetOfferById(offerId);
            var user = await _userRepository.GetUserByEmail(userEmail);

            if (offer.UserId == user.Id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> AddOffer(OfferCreateDto offerDto, string userEmail)
        {
            var validationResult = await _offerValidator.ValidateAsync(offerDto);
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(error => error.ErrorMessage);
                return string.Join(Environment.NewLine, validationErrors);
            }

            var offer = _mapper.Map<Offer>(offerDto);
            offer.EncodeName();
            var user = await _userRepository.GetUserByEmail(userEmail);
            offer.UserId = user.Id;

            if (await _offerRepository.GetOfferBySlug(offer.Slug) != null)
            {
                int randomNumber = new Random().Next(1, 10000);
                offer.Slug = $"{offer.Slug}-{randomNumber}";
            }

            if (await _offerRepository.GetOfferBySlug(offer.Slug) != null)
            {
                return "Name is already in use";
            }

            await _offerRepository.AddOffer(offer);

            return "Offer ceated successful";
        }

        public async Task<string> UpdateOffer(Guid offerId, OfferCreateDto offerDto)
        {
            var validationResult = await _offerValidator.ValidateAsync(offerDto);
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(error => error.ErrorMessage);
                return string.Join(Environment.NewLine, validationErrors);
            }

            var updatedOffer = _mapper.Map<Offer>(offerDto);

            await _offerRepository.UpdateOffer(offerId, updatedOffer);
            return "User updated successful";
        }

        public async Task<string> DeleteOffer(Guid offerId)
        {
            await _offerRepository.DeleteOffer(offerId);
            return "Offer deleted successful";
        }
    }
}
