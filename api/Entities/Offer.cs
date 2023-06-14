namespace ResellHub.Entities
{
    public class Offer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Brand { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int Condition { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public int ProductionYear { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public string Slug { get; set; }

        public User User { get; set; }
        public Category Category { get; set; }
        public List<FollowOffer> FollowingOffers { get; set; }


        public void EncodeName() => Slug = Title.ToLower().Replace(" ", "-");

    }
}
