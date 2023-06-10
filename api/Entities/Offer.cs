namespace ResellHub.Entities
{
    public class Offer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Condition { get; set; }
        public int PricePLN { get; set; }
        public int ProductionYear { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public string EncodedName { get; set; }

        public virtual User User { get; set; }
        public virtual List<FollowOffer> FollowingOffers { get; set; }


        public void EncodeName() => EncodedName = Title.ToLower().Replace(" ", "-");

    }
}
