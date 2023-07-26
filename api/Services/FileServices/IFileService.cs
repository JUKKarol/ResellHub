using ResellHub.DTOs.OfferImageDTOs;

namespace ResellHub.Services.FileServices
{
    public interface IFileService
    {
        //avatars
        Task<byte[]> GetAvatar(Guid userId);
        bool CheckIsAvatarSizeCorrect(IFormFile image);
        Task<bool> AddAvatar(IFormFile image, Guid userId);
        Task<bool> DeleteAvatar(Guid userId);
        //offers
        Task<byte[]> GetOfferImageBySlug(string offerImageSlug);
        Task<OfferImageDisplayDTO> GetOfferPrimaryImage(Guid offerId);
        Task<List<OfferImageDisplayDTO>> GetOfferImagesByOfferId(Guid offerId);
        Task<List<OfferImageDisplayDTO>> GetOfferImagesByOfferSlug(string offerSlug);
        bool CheckIsOfferImageSizeCorrect(IFormFile image);
        Task<bool> CheckIsOfferHaveSpaceForImages(Guid offerId);
        //create
        Task<bool> AddOfferImage(IFormFile image, Guid offerId);
        Task<bool> AddOfferImages(List<IFormFile> images, Guid offerId);
        //delete
        Task<bool> DeleteOfferImage(string offerImageSlug);
    }
}
