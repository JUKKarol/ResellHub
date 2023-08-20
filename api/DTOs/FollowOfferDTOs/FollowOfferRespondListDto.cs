using ResellHub.DTOs.MessageDTOs;

namespace ResellHub.DTOs.FollowOfferDTOs
{
    public class FollowOfferRespondListDto
    {
        public int PagesCount { get; set; }
        public int ItemsCount { get; set; }
        public List<FollowOfferDto> Items { get; set; }
    }
}
