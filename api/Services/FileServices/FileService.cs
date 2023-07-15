using ResellHub.Data.Repositories.UserRepository;
using ResellHub.Entities;
using ResellHub.Services.FileServices;
using ResellHub.Services.UserServices;

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

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
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

    }
}
