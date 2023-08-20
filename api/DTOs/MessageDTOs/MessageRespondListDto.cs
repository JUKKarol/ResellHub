using ResellHub.DTOs.UserDTOs;

namespace ResellHub.DTOs.MessageDTOs
{
    public class MessageRespondListDto
    {
        public int PagesCount { get; set; }
        public int ItemsCount { get; set; }
        public List<MessageDisplayDto> Items { get; set; }
    }
}
