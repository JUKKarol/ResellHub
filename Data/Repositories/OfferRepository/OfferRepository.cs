namespace ResellHub.Data.Repositories.OfferRepository
{
    public class OfferRepository : IOfferRepository
    {
        private readonly ResellHubContext _dbContext;

        public OfferRepository(ResellHubContext resellHub)
        {
            _dbContext = resellHub;
        }


    }
}
