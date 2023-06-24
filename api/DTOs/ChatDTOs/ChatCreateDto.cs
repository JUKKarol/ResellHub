namespace ResellHub.DTOs.ChatDTOs
{
    public class ChatCreateDto
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
    }
}
