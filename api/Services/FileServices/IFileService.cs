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
    }
}
