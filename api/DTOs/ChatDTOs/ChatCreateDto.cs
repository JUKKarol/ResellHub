namespace ResellHub.DTOs.ChatDTOs
{
    public class ChatCreateDto
    {
        public Guid SenderId { get; set; }
        public Guid ReciverId { get; set; }
    }
}