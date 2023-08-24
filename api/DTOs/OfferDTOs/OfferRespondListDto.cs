using ResellHub.DTOs.UserDTOs;

namespace ResellHub.DTOs.OfferDTOs
{
    public class OfferRespondListDto
    {
        public int PagesCount { get; set; }
        public int ItemsCount { get; set; }
        public List<OfferPublicDto> Items { get; set; }
    }
}
