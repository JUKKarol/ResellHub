namespace ResellHub.Data.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ResellHubContext _dbContext;

        public UserRepository(ResellHubContext resellHub)
        {
            _dbContext = resellHub;
        }


    }
}
