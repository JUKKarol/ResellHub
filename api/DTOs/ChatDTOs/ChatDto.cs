namespace ResellHub.DTOs.ChatDTOs
{
    public class ChatDto
    {
        public Guid Id { get; set; }
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public DateTime LastMessageAt { get; set; }
    }
}
