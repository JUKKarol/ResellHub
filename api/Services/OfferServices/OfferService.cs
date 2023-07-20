﻿using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResellHub.Data.Repositories.OfferRepository;
using ResellHub.Data.Repositories.UserRepository;
using ResellHub.DTOs.OfferDTOs;
using ResellHub.DTOs.OfferImageDTOs;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Services.FileServices;
using ResellHub.Utilities.OfferUtilities;
using ResellHub.Utilities.UserUtilities;
using System;

namespace ResellHub.Services.OfferServices
{
    public class OfferService : IOfferService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IFileService _fileService;
        private readonly IOfferUtilities _offerUtilities;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IValidator<OfferCreateDto> _offerValidator;

        public OfferService(IUserRepository userRepository, IOfferRepository offerRepository, IFileService fileService, IOfferUtilities offerUtilities, IConfiguration configuration, IMapper mapper, IValidator<OfferCreateDto> offerValidator)
        {
            _userRepository = userRepository;
            _offerRepository = offerRepository;
            _fileService = fileService;
            _offerUtilities = offerUtilities;
            _configuration = configuration;
            _mapper = mapper;
            _offerValidator = offerValidator;
        }

        //Offer
        public async Task<List<OfferPublicDto>> GetOffers(int page, Guid loggedUserId)
        {
            var offers = await _offerRepository.GetOffers(page, 40);
            var offersDto = _mapper.Map<List<OfferPublicDto>>(offers);

            var followedOfferSlugs = offers
                .Where(offer => offer.FollowingOffers.Any(fo => fo.UserId == loggedUserId))
                .Select(offer => offer.Slug)
                .ToList();

            for (int i = 0; i < offersDto.Count; i++)
            {
                offersDto[i].IsUserFollowing = followedOfferSlugs.Contains(offersDto[i].Slug);
                offersDto[i].OfferPrimaryImage = await _fileService.GetOfferPrimaryImage(offers[i].Id);
            }

            //foreach (var offerDto in offersDto)
            //{
            //    offerDto.IsUserFollowing = followedOfferSlugs.Contains(offerDto.Slug);
            //}

            return await _offerUtilities.ChangeCategoryIdToCategoryName(offersDto);
        }

        public async Task<List<OfferPublicDto>> GetUserOffers(string userSlug, int page, Guid loggedUserId)
        {
            var offers = await _offerRepository.GetUserOffers(userSlug, page, 40);
            var offersDto = _mapper.Map<List<OfferPublicDto>>(offers);

            var followedOfferSlugs = offers
                .Where(offer => offer.FollowingOffers.Any(fo => fo.UserId == loggedUserId))
                .Select(offer => offer.Slug)
                .ToList();

            for (int i = 0; i < offersDto.Count; i++)
            {
                offersDto[i].IsUserFollowing = followedOfferSlugs.Contains(offersDto[i].Slug);
                offersDto[i].OfferPrimaryImage = await _fileService.GetOfferPrimaryImage(offers[i].Id);
            }

            //foreach (var offerDto in offersDto)
            //{
            //    offerDto.IsUserFollowing = followedOfferSlugs.Contains(offerDto.Slug);
            //    offerDto.OfferPrimaryImage = await _fileService.GetOfferPrimaryImage();
            //}

            return await _offerUtilities.ChangeCategoryIdToCategoryName(offersDto);
        }

        public async Task<OfferDetalisDto> GetOfferById(Guid offerId, Guid loggedUserId)
        {
            var offer = await _offerRepository.GetOfferById(offerId);
            var offerDto = _mapper.Map<OfferDetalisDto>(offer);

            var followedOfferSlugs = offer.FollowingOffers
                .Where(fo => fo.UserId == loggedUserId)
                .Select(fo => fo.Offer.Slug)
                .ToList();

            offerDto.IsUserFollowing = followedOfferSlugs.Contains(offerDto.Slug);
            offerDto.OfferImages = await _fileService.GetOfferImagesByOfferId(offerId);

            return await _offerUtilities.ChangeCategoryIdToCategoryName(offerDto);
        }


        public async Task<OfferDetalisDto> GetOfferBySlug(string offerSlug, Guid loggedUserId)
        {
            var offer = await _offerRepository.GetOfferBySlug(offerSlug);
            var offerDto = _mapper.Map<OfferDetalisDto>(offer);

            var followedOfferSlugs = offer.FollowingOffers
                .Where(fo => fo.UserId == loggedUserId)
                .Select(fo => fo.Offer.Slug)
                .ToList();

            offerDto.IsUserFollowing = followedOfferSlugs.Contains(offerDto.Slug);
            offerDto.OfferImages = await _fileService.GetOfferImagesByOfferSlug(offerSlug);

            return await _offerUtilities.ChangeCategoryIdToCategoryName(offerDto);
        }

        public async Task<Guid> GetOfferIdByOfferSlug(string offerSlug)
        {
            var offer = await _offerRepository.GetOfferBySlug(offerSlug);

            return offer.Id;
        }

        public async Task<Offer> GetOfferByOfferImageSlug(string offerImageSlug)
        {
            var offerImage = await _offerRepository.GetOfferImageBySlug(offerImageSlug);
            var offer = await _offerRepository.GetOfferById(offerImage.OfferId);

            return offer;
        }

        public async Task<Guid> GetOfferIdByOfferImageSlug(string offerImageSlug)
        {
            var offerImage = await _offerRepository.GetOfferImageBySlug(offerImageSlug);

            return offerImage.OfferId;
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
            return "User updated successful";
        }

        public async Task<string> DeleteOffer(Guid offerId)
        {
            await _offerRepository.DeleteOffer(offerId);
            return "Offer deleted successful";
        }

        //Image
        public async Task<string> SetOfferImageAsPrimaryBySlug(string offerImageSlug)
        {
            await _offerRepository.SetOfferImageAsPrimaryBySlug(offerImageSlug);

            return offerImageSlug;
        }
    }
}
