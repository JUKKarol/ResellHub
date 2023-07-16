using ResellHub.Data.Repositories.UserRepository;
using ResellHub.Entities;
using ResellHub.Services.FileServices;
using ResellHub.Services.UserServices;
using System.IO;

namespace ResellHub.Services.FileService
{
    public class FileService : IFileService
    {
        private readonly string imagesFolderPath = $"{Directory.GetParent(Environment.CurrentDirectory).FullName}\\Images";
        private readonly IUserRepository _userRepository;

        public FileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //get
        private async Task<byte[]> GetImage(string imageName, string imagesFolder)
        {
            try
            {
                string targetFolderPath = $"{imagesFolderPath}\\{imagesFolder}";
                Directory.CreateDirectory(targetFolderPath);

                DirectoryInfo directory = new DirectoryInfo(targetFolderPath);
                FileInfo[] files = directory.GetFiles(imageName + ".*");

                foreach (FileInfo file in files)
                {
                    using (var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        await fileStream.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        return memoryStream.ToArray();
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<byte[]> GetAvatar(Guid userId)
        {
            return await GetImage(userId.ToString(), "Avatars");
        }

        //check
        private bool ChceckIsImageSizeCorrect(IFormFile image, int MaxsizeInMegaBytes)
        {
            int maxSizeInBytes = MaxsizeInMegaBytes * 1024 * 1024;

            if (image.Length > maxSizeInBytes)
            { 
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckIsAvatarSizeCorrect(IFormFile image)
        {
            return ChceckIsImageSizeCorrect(image, 1);
        }

        //create
        private async Task<string> AddImage(IFormFile image, string imageName, string imagesFolder)
        {
            try
            {
                string targetFolderPath = $"{imagesFolderPath}\\{imagesFolder}";
                Directory.CreateDirectory(targetFolderPath);

                var lastIndexOfDot = image.FileName.LastIndexOf('.');
                string extension = image.FileName.Substring(lastIndexOfDot + 1);

                imageName = $"{imageName}.{extension}";

                string filePath = Path.Combine(targetFolderPath, imageName);

                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await image.CopyToAsync(fileStream);
                }

                return imageName;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public async Task<bool> AddAvatar(IFormFile image, Guid userId)
        {
            string imageSlug = await AddImage(image, userId.ToString(), "Avatars");

            if (imageSlug == null)
            {
                return false;
            }
            var avatarImage = new AvatarImage()
            {
                UserId = userId,
                ImageSlug = imageSlug,
            };

            await _userRepository.AddAvatarImage(avatarImage);

            return true;
        }

        //delete
        private async Task<bool> DeleteImage(string imageName, string imagesFolder)
        {
            try
            {
                string targetFolderPath = $"{imagesFolderPath}\\{imagesFolder}";
                Directory.CreateDirectory(targetFolderPath);

                DirectoryInfo directory = new DirectoryInfo(targetFolderPath);
                FileInfo[] files = directory.GetFiles(imageName + ".*");

                foreach (FileInfo file in files)
                {
                    await Task.Run(() => file.Delete());
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAvatar(Guid userId)
        {
            bool deleteCorrect = await DeleteImage(userId.ToString(), "Avatars");

            if (!deleteCorrect)
            {
                return false;
            }

            await _userRepository.DeleteAvatarImage(userId);

            return true;
        }
    }
}
