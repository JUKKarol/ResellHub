using AutoMapper;
using ResellHub.Data.Repositories.UserRepository;
using ResellHub.Utilities.UserUtilities;

namespace ResellHub.Services.OfferServices
{
    public class OfferService : IOfferService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserUtilities _userService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public OfferService(IUserRepository userRepository, IUserUtilities userService, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _userService = userService;
            _configuration = configuration;
            _mapper = mapper;
        }




    }
}
