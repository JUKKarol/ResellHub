namespace ResellHub.Services.FileServices
{
    public interface IFileService
    {
        Task<byte[]> GetAvatar(Guid userId);
        Task<bool> AddAvatar(IFormFile image, Guid userId);
        Task<bool> DeleteAvatar(Guid userId);
    }
}
