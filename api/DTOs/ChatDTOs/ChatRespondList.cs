using ResellHub.DTOs.FollowOfferDTOs;

namespace ResellHub.DTOs.ChatDTOs
{
    public class ChatRespondList
    {
        public int PagesCount { get; set; }
        public int ItemsCount { get; set; }
        public List<ChatDisplayDto> Items { get; set; }
    }
}
