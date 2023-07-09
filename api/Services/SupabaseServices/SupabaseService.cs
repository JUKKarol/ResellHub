using System.IO;
using System.Threading.Tasks;
using ResellHub.Data.Repositories.UserRepository;
using Supabase;
using Supabase.Interfaces;
using Supabase.Storage;
using Client = Supabase.Client;

namespace ResellHub.Services.SupabaseServices
{
    public class SupabaseService : ISupabaseService
    {
        private readonly Client _client;
        private readonly IUserRepository _userRepository;

        public SupabaseService(Client client, IUserRepository userRepository)
        {
            _client = client;
            _userRepository = userRepository;
        }

        public async Task<string> UploadImage(string bucketName, string fileName, IFormFile image)
        {
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            var photoBytes = memoryStream.ToArray();

            var result = await _client.Storage
                .From(bucketName)
                .Upload(photoBytes, fileName);

            return result;
        }

        public async Task<byte[]> DownloadImage(string bucketName, string fileName)
        {
            var options = new TransformOptions
            {
                Width = 800,
                Height = 600
            };

            var bytes = await _client.Storage.From(bucketName).Download(fileName, options);

            return bytes;
        }

        public async Task<bool> UploadAvatar(IFormFile photo, Guid userGuid)
        {
            var lastIndexOfDot = photo.FileName.LastIndexOf('.');
            string extension = photo.FileName.Substring(lastIndexOfDot + 1);

            string fileName = $"{userGuid}.{extension}";
            string avatarsBucketName = "user-avatars";

            var photoUrl = await UploadImage(avatarsBucketName, fileName, photo);

            if (photoUrl.Length < 1)
            { 
                return false;
            }

            await _userRepository.AddAvatarImage(photoUrl, userGuid);

            return true;
        }

        public async Task<bool> CheckIsImageExist(string bucketName, string fileName)
        {
            var options = new TransformOptions
            {
                Width = 800,
                Height = 600
            };

            var bytes = await _client.Storage.From(bucketName).Download(fileName, options);

            if (bytes.Length > 0)
            {
                return true;
            }
            else
            { 
                return false;
            }
        }

        public async Task<bool> CheckIsAvatarExist(string fileName)
        {
            return await CheckIsImageExist("user-avatars", fileName);
        }
    }
}
