namespace ResellHub.Entities
{
    public class Offer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public byte[] Image1 { get; set; }
        public byte[] Image2 { get; set; }
        public byte[] Image3 { get; set; }

        public string Title { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Condition { get; set; }
        public int PricePLN { get; set; }
        public int ProductionYear { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid UserId { get; set; }
        public string EncodedName { get; set; }

        public User User { get; set; }
        public List<FollowOffer> FollowingOffers { get; set; }


        public void EncodeName() => EncodedName = Title.ToLower().Replace(" ", "-");

    }
}
