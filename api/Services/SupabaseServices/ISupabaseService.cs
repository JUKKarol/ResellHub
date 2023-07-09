namespace ResellHub.Services.SupabaseServices
{
    public interface ISupabaseService
    {
        Task<string> UploadImage(string bucketName, string fileName, IFormFile image);
        Task<bool> UploadAvatar(IFormFile photo, Guid userGuid);
        Task<byte[]> DownloadImage(string bucketName, string fileName);
        Task<bool> CheckIsImageExist(string bucketName, string fileName);
        Task<bool> CheckIsAvatarExist(string fileName);
    }
}
