using ResellHub.Enums;
using System.Text.RegularExpressions;

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
        public Currencies Currency { get; set; } = Currencies.PLN;
        public int ProductionYear { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public string Slug { get; set; }

        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<FollowOffer> FollowingOffers { get; set; }
        public virtual List<OfferImage> OfferImages { get; set; }

        public void EncodeName() => Slug = $"{Regex.Replace(Title, @"[^a-zA-Z0-9]", "").ToLower()}-{Id.ToString().Substring(0, 4)}";
    }
}
