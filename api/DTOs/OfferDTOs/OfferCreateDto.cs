using ResellHub.Enums;
using System.ComponentModel.DataAnnotations;

namespace ResellHub.DTOs.OfferDTOs
{
    public class OfferCreateDto
    {
        public string Title { get; set; }
        public string Brand { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int Condition { get; set; }
        public int Price { get; set; }
        public Currencies Currency { get; set; }
        public int ProductionYear { get; set; }
        public Guid UserId { get; set; }
    }
}
