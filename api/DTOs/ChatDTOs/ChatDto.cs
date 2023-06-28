namespace ResellHub.DTOs.ChatDTOs
{
    public class ChatDto
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReciverId { get; set; }
        public DateTime LastMessageAt { get; set; }
    }
}
