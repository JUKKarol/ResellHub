using ResellHub.Enums;

namespace ResellHub.DTOs.OfferDTOs
{
    public class OfferPublicDto
    {
        public string Title { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Condition { get; set; }
        public int Price { get; set; }
        public Currencies Currency { get; set; }
        public int ProductionYear { get; set; }
        public Guid UserId { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
