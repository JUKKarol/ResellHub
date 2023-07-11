using System.IO;
using System.Threading.Tasks;
using ResellHub.Data.Repositories.UserRepository;
using ResellHub.Entities;
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

        public async Task<bool> DeleteImage(string bucketName, string fileName)
        {
            var result = await _client.Storage.From(bucketName).Remove(fileName);

            if (result != null)
            {
                return true;
            }
            else
            { 
                return false;
            }
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

        public async Task<bool> UploadAvatar(IFormFile photo, Guid userId)
        {
            var lastIndexOfDot = photo.FileName.LastIndexOf('.');
            string extension = photo.FileName.Substring(lastIndexOfDot + 1);

            string fileName = $"{userId}.{extension}";
            string avatarsBucketName = "user-avatars";

            var photoUrl = await UploadImage(avatarsBucketName, fileName, photo);

            if (photoUrl.Length < 1)
            { 
                return false;
            }

            var avatarImage = new AvatarImage()
            {
                ImageSlug = photoUrl,
                UserId = userId,
            };

            await _userRepository.AddAvatarImage(avatarImage);

            return true;
        }

        public async Task<bool> DeleteAvatar(string fileName, Guid userId)
        {
            var result = await _client.Storage.From("user-avatars").Remove(fileName);

            if (result == null)
            {
                return false;
            }

            await _userRepository.DeleteAvatarImage(userId);

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
