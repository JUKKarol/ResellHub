namespace ResellHub.Entities
{
    public class FollowOffer
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public Guid OfferId { get; set; }

        public User User { get; set; }
        public Offer Offer { get; set; }
    }
}
