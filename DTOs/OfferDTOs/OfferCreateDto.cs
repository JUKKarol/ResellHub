using System.ComponentModel.DataAnnotations;

namespace ResellHub.DTOs.OfferDTOs
{
    public class OfferCreateDto
    {
        public OfferCreateDto()
        {
            int year = DateTime.Now.Year;
        }

        [Required]
        [StringLength(30, ErrorMessage = "Title can be up to 30 characters long")]
        [MinLength(4, ErrorMessage = "Title must be at least 4 characters long")]
        public string Title { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Title can be up to 20 characters long")]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters long")]
        public string Brand { get; set; }
        public string Category { get; set; }
        [StringLength(200, ErrorMessage = "Description can be up to 200 characters long")]
        public string Description { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Condition must be between 1 and 5")]
        public int Condition { get; set; }
        [Required]
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10 000")]
        public int PricePLN { get; set; }
        [Required]
        [Range(1950, 2030, ErrorMessage = $"Incorrect year")]
        public int ProductionYear { get; set; }
        public Guid UserId { get; set; }
    }
}
