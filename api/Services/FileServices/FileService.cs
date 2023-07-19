using AutoMapper;
using ResellHub.Data.Repositories.OfferRepository;
using ResellHub.Data.Repositories.UserRepository;
using ResellHub.DTOs.OfferImageDTOs;
using ResellHub.Entities;
using ResellHub.Services.FileServices;
using ResellHub.Services.UserServices;
using System.IO;

namespace ResellHub.Services.FileService
{
    public class FileService : IFileService
    {
        private readonly string imagesFolderPath = $"{Directory.GetParent(Environment.CurrentDirectory).FullName}\\Images";
        private readonly string avatarsFolderName = "Avatars";
        private readonly string offerImagesFolderName = "OfferImages";

        private readonly IUserRepository _userRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;

        public FileService(IUserRepository userRepository, IOfferRepository offerRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _offerRepository = offerRepository;
            _mapper = mapper;
        }

        //get
        private async Task<byte[]> GetImage(string imageName, string imagesFolder)
        {
            try
            {
                string targetFolderPath = $"{imagesFolderPath}\\{imagesFolder}";
                Directory.CreateDirectory(targetFolderPath);

                DirectoryInfo directory = new DirectoryInfo(targetFolderPath);

                if (imageName.Contains("."))
                {
                    FileInfo file = new FileInfo(Path.Combine(targetFolderPath, imageName));

                    using (var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        await fileStream.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        return memoryStream.ToArray();
                    }
                }

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
            return await GetImage(userId.ToString(), avatarsFolderName);
        }

        public async Task<byte[]> GetOfferImageBySlug(string offerImageSlug)
        {
            return await GetImage(offerImageSlug, offerImagesFolderName);
        }

        public async Task<OfferImageDisplayDTO> GetOfferPrimaryImage(Guid offerId)
        {
            OfferImage primaryImage = await _offerRepository.GetPrimaryOfferImageByOfferId(offerId);

            if (primaryImage == null)
            {
                return new OfferImageDisplayDTO();
            }

            var primaryImageDto = new OfferImageDisplayDTO()
            {
                ImageSlug = primaryImage.ImageSlug,
                ImageBytes = await GetImage(primaryImage.ImageSlug, offerImagesFolderName),
            };

            return primaryImageDto;
        }

        public async Task<List<OfferImageDisplayDTO>> GetOfferImagesByOfferId(Guid offerId)
        {
            var offerImages = await _offerRepository.GetAllOfferImagesByOfferId(offerId);
            var offerImagesDto = _mapper.Map<List<OfferImageDisplayDTO>>(offerImages);

            if (offerImages == null)
            {
                return new List<OfferImageDisplayDTO>();
            }

            foreach (var offerImageDto in offerImagesDto)
            {
                offerImageDto.ImageBytes = await GetImage(offerImageDto.ImageSlug, offerImagesFolderName);
            }

            return offerImagesDto;
        }

        public async Task<List<OfferImageDisplayDTO>> GetOfferImagesByOfferSlug(string offerSlug)
        {
            var offerImages = await _offerRepository.GetAllOfferImagesByOfferslug(offerSlug);
            var offerImagesDto = _mapper.Map<List<OfferImageDisplayDTO>>(offerImages);

            if (offerImages == null)
            {
                return new List<OfferImageDisplayDTO>();
            }

            foreach (var offerImageDto in offerImagesDto)
            {
                offerImageDto.ImageBytes = await GetImage(offerImageDto.ImageSlug, offerImagesFolderName);
            }

            return offerImagesDto;
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

        public bool CheckIsOfferImageSizeCorrect(IFormFile image)
        {
            return ChceckIsImageSizeCorrect(image, 3);
        }

        public async Task<bool> CheckIsOfferHaveSpaceForImages(Guid offerId)
        {
            var offerImages = await _offerRepository.GetAllOfferImagesByOfferId(offerId);
            int offerImagesCount = offerImages.Count;

            if (offerImagesCount > 3)
            {
                return false;
            }
            else
            {
                return true;
            }
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
            string imageSlug = await AddImage(image, userId.ToString(), avatarsFolderName);

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

        public async Task<bool> AddOfferImage(IFormFile image, Guid offerId)
        {
            Guid offerImageId = Guid.NewGuid();
            string imageSlug = await AddImage(image, offerImageId.ToString(), offerImagesFolderName);

            if (imageSlug == null)
            {
                return false;
            }

            var offerImage = new OfferImage()
            {
                Id = offerImageId,
                ImageSlug = imageSlug,
                OfferId = offerId,
            };

            await _offerRepository.AddOfferImageImage(offerImage);

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
            bool deleteCorrect = await DeleteImage(userId.ToString(), avatarsFolderName);

            if (!deleteCorrect)
            {
                return false;
            }

            await _userRepository.DeleteAvatarImage(userId);

            return true;
        }

        public async Task<bool> DeleteOfferImage(Guid offerImageId)
        {
            bool deleteCorrect = await DeleteImage(offerImageId.ToString(), offerImagesFolderName);

            if (!deleteCorrect)
            {
                return false;
            }

            await _offerRepository.DeleteOfferImage(offerImageId);

            return true;
        }
    }
}
