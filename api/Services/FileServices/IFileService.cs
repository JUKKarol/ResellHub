namespace ResellHub.Services.FileServices
{
    public interface IFileService
    {
        Task<bool> AddAvatar(IFormFile image, Guid userId);
    }
}
