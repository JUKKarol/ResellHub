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
        Task<byte[]> GetOfferPrimaryImage(Guid offerId);
        Task<List<OfferImageDisplayDTO>> GetOfferImages(Guid offerId);
        Task<bool> CheckIsOfferHaveSpaceForImages(Guid offerId);
        Task<bool> AddOfferImage(IFormFile image, Guid offerId);
        Task<bool> DeleteOfferImage(Guid offerImageId);
    }
}
