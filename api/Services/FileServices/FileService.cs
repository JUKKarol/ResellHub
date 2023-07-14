using ResellHub.Services.FileServices;
using ResellHub.Services.UserServices;

namespace ResellHub.Services.FileService
{
    public class FileService : IFileService
    {
        private readonly string projectDirectoryPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        private readonly IUserService _userService;

        public FileService(IUserService userService)
        {
            _userService = userService;
        }



    }
}
