namespace ResellHub.Entities
{
    public class OfferImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ImageSlug { get; set; }
        public Guid OfferId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual Offer Offer { get; set; }
    }
}