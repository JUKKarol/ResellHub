namespace ResellHub.DTOs.MessageDTOs
{
    public class MessageDisplayDto
    {
        public Guid SenderId { get; set; }
        public Guid ReciverId { get; set; }

        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}