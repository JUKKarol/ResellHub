using AutoMapper;
using FluentValidation;
using ResellHub.Data.Repositories.OfferRepository;
using ResellHub.Data.Repositories.UserRepository;
using ResellHub.DTOs.OfferDTOs;
using ResellHub.DTOs.UserDTOs;
using ResellHub.Entities;
using ResellHub.Utilities.UserUtilities;

namespace ResellHub.Services.OfferServices
{
    public class OfferService : IOfferService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IUserUtilities _userService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IValidator<OfferCreateDto> _offerValidator;

        public OfferService(IUserRepository userRepository, IOfferRepository offerRepository, IUserUtilities userUtilities, IConfiguration configuration, IMapper mapper, IValidator<OfferCreateDto> offerValidator)
        {
            _userRepository = userRepository;
            _offerRepository = offerRepository;
            _userService = userUtilities;
            _configuration = configuration;
            _mapper = mapper;
            _offerValidator = offerValidator;
        }

        public async Task<List<OfferPublicDto>> GetOffers()
        {
            var offers = await _offerRepository.GetOffers();
            var offersDto = _mapper.Map<List<OfferPublicDto>>(offers);

            return offersDto;
        }

        public async Task<List<OfferPublicDto>> GetUserOffers(Guid userId)
        {
            var offers = await _offerRepository.GetUserOffers(userId);
            var offersDto = _mapper.Map<List<OfferPublicDto>>(offers);

            return offersDto;
        }

        public async Task<OfferPublicDto> GetOfferById(Guid offerId)
        {
            var offer = await _offerRepository.GetOfferById(offerId);
            var offerDto = _mapper.Map<OfferPublicDto>(offer);

            return offerDto;
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

            if (await _offerRepository.GetOfferByEncodedName(offer.EncodedName) != null)
            {
                int randomNumber = new Random().Next(1, 10000);
                offer.EncodedName = $"{offer.EncodedName}-{randomNumber}";
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
